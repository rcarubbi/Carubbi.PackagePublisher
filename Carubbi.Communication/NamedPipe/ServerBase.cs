using Carubbi.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carubbi.Communication.NamedPipe
{
    public abstract class ServerBase<TEntradaServico, TSaidaServico> : Form
        where TEntradaServico : class
    {
        public int SegundosOcioso { get; set; }

        private BackgroundWorker _bgEscutarPipe;
        private BackgroundWorker _bgStillAlive;
        private NamedPipeServerStream _serverPipe;
        private StreamReader _sr;

        private NamedPipeClientStream _callbackPipe;
        private StreamWriter _sw;

        private static DateTime _dataUltimaRequisicao;
        private bool _stillAliveEmExecucao = false;

        protected abstract TSaidaServico ExecutarServico(TEntradaServico entradaServico);

        protected abstract void Login();

        protected abstract void StillAlive();

        public ServerBase(string nomePipeInput, string nomePipeOutput)
        {
            _bgEscutarPipe = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            _bgStillAlive = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            _serverPipe = new NamedPipeServerStream(nomePipeInput, System.IO.Pipes.PipeDirection.In, 1);
            _sr = new StreamReader(_serverPipe);
            _callbackPipe = new NamedPipeClientStream(".", nomePipeOutput, PipeDirection.Out);
            _sw = new StreamWriter(_callbackPipe);
            
            this.Shown += ServerBase_Shown;
        }

        void ServerBase_Shown(object sender, EventArgs e)
        {
            Login();
            _dataUltimaRequisicao = DateTime.Now;
            InicializarStillAlive();
            EscutarNamedPipe();
        }

        protected virtual void EscutarNamedPipe()
        {
            _bgEscutarPipe.DoWork += bgEscutarPipe_DoWork;
            _bgEscutarPipe.RunWorkerCompleted += _bgEscutarPipe_RunWorkerCompleted;
            _bgEscutarPipe.RunWorkerAsync();
        }

        protected void _bgEscutarPipe_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _bgEscutarPipe.RunWorkerAsync();
        }

        protected void bgEscutarPipe_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_bgEscutarPipe.CancellationPending)
            {
                if (!_serverPipe.IsConnected)
                    _serverPipe.WaitForConnection();

                string msg = _sr.ReadLine();

                if (msg == null )
                {
                    _serverPipe.Disconnect();
                    _bgEscutarPipe.CancelAsync();
                    continue;
                }

                var serializadorInput = new Serializer<List<TEntradaServico>>();
                List<TEntradaServico> lote = serializadorInput.XmlDeserialize(msg);
                
                foreach (var item in lote)
                {
                    while (_stillAliveEmExecucao)
                    {
                        Thread.Sleep(200);
                    }
                    TSaidaServico output = ExecutarServico(item);
                    _dataUltimaRequisicao = DateTime.Now;
                    CallBack(output);
                }
               
            }

            if (_serverPipe.IsConnected)
            {
                _serverPipe.Disconnect();
            }
        }

        private void CallBack(TSaidaServico retorno)
        {
            var serializadorOutput = new Serializer<TSaidaServico>();
            var mensagemCallBack = serializadorOutput.XmlSerialize(retorno).Replace(Environment.NewLine, string.Empty);          

            if (!_callbackPipe.IsConnected)
            {
                _callbackPipe.Connect();
                _sw.AutoFlush = true;
            } 

            _sw.WriteLine(mensagemCallBack);
            _callbackPipe.WaitForPipeDrain();
        }
         
        private void InicializarStillAlive()
        {
            _bgStillAlive.DoWork += _bgStillAlive_DoWork;
            _bgStillAlive.RunWorkerAsync();
        }

        protected void _bgStillAlive_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_bgStillAlive.CancellationPending)
            {
                if (_dataUltimaRequisicao.AddSeconds(SegundosOcioso) < DateTime.Now)
                {
                    _stillAliveEmExecucao = true;
                    StillAlive();
                    _dataUltimaRequisicao = DateTime.Now;
                    _stillAliveEmExecucao = false;
                }
            }
        }
    }
}
