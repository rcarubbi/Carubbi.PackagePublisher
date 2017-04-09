using Carubbi.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Web;

namespace Carubbi.Communication.NamedPipe
{
    public class NamedPipeClient<TDTOInput, TDTOOutput, TDTOCredenciais> : IObservable<TDTOOutput>, IDisposable
        where TDTOInput : class
        where TDTOOutput : class
        where TDTOCredenciais : class
    {
        private Process _processoServidor;
        private List<IObserver<TDTOOutput>> _observers;
        private BackgroundWorker _bgCallback;
        private NamedPipeClientStream _clientPipe;
        private NamedPipeServerStream _callbackPipe;
        private StreamWriter _sw;
        private StreamReader _sr;
        private string _nomeIntegracao = string.Empty;
        int _loteCount = 0;

        public NamedPipeClient(string nomePipeInput, string nomePipeOutput, TDTOCredenciais credenciais, string nomeIntegracao)
        {
            _clientPipe = new NamedPipeClientStream(".", nomePipeInput, System.IO.Pipes.PipeDirection.Out);
            _callbackPipe = new NamedPipeServerStream(nomePipeOutput, System.IO.Pipes.PipeDirection.In, 1);
            _sw = new StreamWriter(_clientPipe);
            _sr = new StreamReader(_callbackPipe);
            _observers = new List<IObserver<TDTOOutput>>();
            _nomeIntegracao = nomeIntegracao;
            InicializarServidor(credenciais);
            InicializarCallbackListener();
        }

        ~NamedPipeClient()
        {
            Dispose();
        }

        private void InicializarServidor(TDTOCredenciais credenciais)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = ConfigurationManager.AppSettings[string.Format("CAMINHO_SERVIDOR_{0}", _nomeIntegracao)];


            if (!ServidorEmExecucao)
            {
                Serializer<TDTOCredenciais> serializador = new Serializer<TDTOCredenciais>();
                startInfo.Arguments = String.Concat("\"", HttpUtility.HtmlEncode(serializador.XmlSerialize(credenciais).Replace(Environment.NewLine, string.Empty)), "\"");
                startInfo.CreateNoWindow = true;
                _processoServidor = Process.Start(startInfo);
            }
        }

        private Process RecuperarProcessoServidor()
        {
            if (ServidorEmExecucao)
                return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ConfigurationManager.AppSettings[string.Format("CAMINHO_SERVIDOR_{0}", _nomeIntegracao)]))[0];
            else
                return null;
        }

        public bool ServidorEmExecucao
        {
            get
            {
                return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ConfigurationManager.AppSettings[string.Format("CAMINHO_SERVIDOR_{0}", _nomeIntegracao)])).Length > 0;
            }
        }

        private void InicializarCallbackListener()
        {
            _bgCallback = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            _bgCallback.DoWork += _bgCallback_DoWork;
            _bgCallback.RunWorkerCompleted += _bgCallback_RunWorkerCompleted;
            _bgCallback.RunWorkerAsync();
        }

        protected void _bgCallback_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _bgCallback.RunWorkerAsync();
        }

        protected void _bgCallback_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_bgCallback.CancellationPending)
            {
                if (!_callbackPipe.IsConnected)
                    _callbackPipe.WaitForConnection();

                string msg = _sr.ReadLine();

                if (msg == null)
                {
                    _callbackPipe.Disconnect();
                    _bgCallback.CancelAsync();
                    continue;
                }

                var serializador = new Serializer<TDTOOutput>();
                TDTOOutput item = serializador.XmlDeserialize(msg);
                NotificarItem(item);
                _loteCount--;

                if (_loteCount == 0)
                    NotificarFinalProcessamentoLote();
            }

            if (_callbackPipe.IsConnected)
            {
                _callbackPipe.Disconnect();
            }
        }

        private void NotificarFinalProcessamentoLote()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }

        }

        private void NotificarItem(TDTOOutput item)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(item);
            }
        }

        public IDisposable Subscribe(IObserver<TDTOOutput> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            _observers.Add(observer);

            return new Unsubscriber<TDTOOutput>(_observers, observer);
        }

        public void Enviar(List<TDTOInput> lote)
        {
            if (!_clientPipe.IsConnected)
            {
                _clientPipe.Connect();
                _sw.AutoFlush = true;
            }

            _loteCount = lote.Count;

            var serializador = new Serializer<List<TDTOInput>>();
            var msg = serializador.XmlSerialize(lote);
            msg = msg.Replace(Environment.NewLine, string.Empty);
            _sw.WriteLine(msg);

            _clientPipe.WaitForPipeDrain();
        }

        public void Dispose()
        {
            _observers.Clear();
            _bgCallback.CancelAsync();

            _clientPipe.Close();
            _clientPipe.Dispose();

            if (_callbackPipe.IsConnected)
                _callbackPipe.Disconnect();

            _callbackPipe.Close();
            _callbackPipe.Dispose();

            EncerrarServidor();
        }

        private void EncerrarServidor()
        {
            if (ServidorEmExecucao)
            {
                RecuperarProcessoServidor().Kill();
            }
        }

        public object Serializer { get; set; }
    }
}
