using Carubbi.Mainframe.Config;
using Carubbi.Mainframe.Exeptions;
using Carubbi.Utils.Data;
using Carubbi.Utils.IoC;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Carubbi.Mainframe
{

    /// <summary>
    /// Controle de Fachada utilizado como interface entre a aplicação cliente e o componente de emulação mainframe (que pode ser o MicroFocus Rumba ou o Simulador de emulação Carubbi)
    /// </summary>
    public partial class MFDisplay : UserControl, IDisposable
    {
        #region Variáveis Privadas
        private Size BASE_SIZE = new Size { Width = 480, Height = 320 };
        private const int TIMEOUT = 4000;
        private const int SLEEP = 50;
        private DateTime _startTime = DateTime.Now;
        private string _lastScreen = "";
        private MainframeConfigElement _terminalConfig = new MainframeConfigElement();
        private const string DEFAULT_MAINFRAMECONFIG = "Mainframe.config";
        #endregion

        #region Construtores

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadConfig();
        }


        public MFDisplay()
        {
            InitializeComponent();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MFDisplay));
          
            if (this.IsInDesignMode)
            {
                this.objXMFDisplay = new Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay();
            }
            else
            {
                this.objXMFDisplay = ImplementationResolver.Resolve("AxObjectXMFDisplay");
            }

            ((System.ComponentModel.ISupportInitialize)(this.objXMFDisplay)).BeginInit();

            this.SuspendLayout();
            // 
            // objXMFDisplay
            // 
            if (this.IsInDesignMode)
            {
                (this.objXMFDisplay as Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay).Dock = System.Windows.Forms.DockStyle.Fill;
                (this.objXMFDisplay as Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay).Enabled = true;
                (this.objXMFDisplay as Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay).Location = new System.Drawing.Point(0, 0);
                (this.objXMFDisplay as Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay).Name = "objXMFDisplay";
                (this.objXMFDisplay as Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay).OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("objXMFDisplay.OcxState")));
                (this.objXMFDisplay as Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay).Size = new System.Drawing.Size(480, 320);
                (this.objXMFDisplay as Carubbi.ObjectXMFDisplay.AxObjectXMFDisplay).TabIndex = 0;
            }
            else
            {
                this.objXMFDisplay.Call("CreateControl");
                this.objXMFDisplay.SetProperty("Dock", System.Windows.Forms.DockStyle.Fill);
                this.objXMFDisplay.SetProperty("Enabled", true);
                this.objXMFDisplay.SetProperty("Location", new System.Drawing.Point(0, 0));
                this.objXMFDisplay.SetProperty("Name", "objXMFDisplay");
                this.objXMFDisplay.SetProperty("OcxState", ((System.Windows.Forms.AxHost.State)(resources.GetObject("objXMFDisplay.OcxState"))));
                this.objXMFDisplay.SetProperty("Size", new System.Drawing.Size(480, 320));
                this.objXMFDisplay.SetProperty("TabIndex", 0);
                this.objXMFDisplay.SetProperty("EventVersion", 1);
                this.objXMFDisplay.SetProperty("CharacterSetID2", 32);
                this.objXMFDisplay.SetProperty("FontTypeFace", "Term3270");
                this.objXMFDisplay.SetProperty("AutoFont", true);
                this.objXMFDisplay.SetProperty("AutoFontMinimumWidth", 2);
                this.objXMFDisplay.SetProperty("CharacterCase", false);
                this.objXMFDisplay.SetProperty("ClipboardConfiguration", 0);
                this.objXMFDisplay.SetProperty("CursorBlinkRate", 1000);
                this.objXMFDisplay.SetProperty("ShowLightPenCursor", true);
                this.objXMFDisplay.SetProperty("WatermarkConfiguration", 0);
            }

            this.Controls.Add(this.objXMFDisplay as Control);
            ((System.ComponentModel.ISupportInitialize)(this.objXMFDisplay)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        #region Propriedades
        public bool IsInDesignMode
        {
            get
            {
                return base.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }

        private MainframeConfigElement TerminalConfig
        {
            get
            {
                var section = (MainframeConfigSection)System.Configuration.ConfigurationManager.GetSection("Mainframe");
                
                foreach(MainframeConfigElement terminal in section.MainframeTerminals)
                {
                    if (terminal.Nome == this.Name)
                    {
                        _terminalConfig = terminal;
                        break;
                    }
                }
                
                return _terminalConfig;
            }
            set { _terminalConfig = value; }
        }

        public string ConfigName { get; set; }

        public bool HostConnected { get { return objXMFDisplay.GetProperty<bool>("HostConnected"); } }

        public int CursorRow
        {
            get
            {
                return objXMFDisplay.GetProperty<int>("CursorRow");
            }
            set
            {
                objXMFDisplay.SetProperty("CursorRow", value);
            }
        }

        public int CursorColumn
        {
            get
            {
                return objXMFDisplay.GetProperty<int>("CursorColumn");
            }
            set
            {
                objXMFDisplay.SetProperty("CursorColumn", value);
            }
        }

        #endregion

        #region Métodos Privados

        public void LoadConfig()
        {
            ConfigName = this.Name;
            objXMFDisplay.Call("LoadConfig", ConfigName);
        }
     
        private static void ConfigureDisplay()
        {

        }

        void IDisposable.Dispose()
        {
            Disconnect();
        }

        private void SetStart()
        {
            _startTime = DateTime.Now;
        }

        private bool CheckTimeout(int timeout)
        {
            return _startTime.AddMilliseconds(timeout > 0? timeout: TIMEOUT) < DateTime.Now;
        }

        private bool CheckScreenChange()
        {
            return _lastScreen.Trim() != GetScreen().Trim();
        }

        private void SetLastScreen()
        {
            Sleep(SLEEP);
            Application.DoEvents();
            _lastScreen = GetScreen();
        }

        private void Sleep(int milisecondsTimeOut)
        {
            System.Threading.Thread.Sleep(milisecondsTimeOut);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Aguarda até que uma alteração ocorra na tela ou até que o tempo de espera seja atingido
        /// </summary>
        public void WaitForChanges()
        {
            SetStart();
            while (!CheckTimeout(0) && !CheckScreenChange())
            {
                Sleep(SLEEP);
                Application.DoEvents();
            }
            if (!CheckScreenChange())
            {
                throw new Exception("Timeout");
            }
            SetLastScreen();
        }

        private bool firstConenction = true;

        /// <summary>
        /// Efetua a conexão com o terminal mainframe
        /// </summary>
        public void Connect()
        {
            LoadConfig();
            if (objXMFDisplay.GetProperty<bool>("HostConnected"))
            {
                objXMFDisplay.Call("Disconnect");
                Sleep(SLEEP);
            }

            objXMFDisplay.SetProperty("HostInterfaceConfiguration", 15);

            if (firstConenction)
            {
                Thread t = new Thread(ConfigureDisplay);
                t.Start();
                firstConenction = false;
            }
            objXMFDisplay.Call("Connect");

            SetStart();
            while (!CheckTimeout(16000) && !objXMFDisplay.GetProperty<bool>("HostConnected"))
            {
                WaitForChanges();
                Sleep(SLEEP);
                Application.DoEvents();
            }
            if (!objXMFDisplay.GetProperty<bool>("HostConnected"))
            {
                throw new Exception("Timeout");
            }
            SetLastScreen();

        }

        /// <summary>
        /// Desconecta do terminal
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (objXMFDisplay.GetProperty<bool>("HostConnected"))
                    objXMFDisplay.Call("Disconnect");
            }
            catch { }
        }


        /// <summary>
        /// Verifica se um determinado texto se encontra em uma coordenada na tela atual
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="colum"></param>
        /// <returns></returns>
        public bool CompareText(string text, int row, int colum)
        {
            return text.ToUpper() == GetScreen(row, colum, text.Length).ToUpper();
        }


        /// <summary>
        /// Retorna o texto da tela corrente
        /// </summary>
        /// <returns></returns>
        public string GetScreen()
        {
            return GetScreen(1, 1, 1920);
        }


        /// <summary>
        /// Retorna parte do texto na tela corrente a partir de uma coordenada e do tamanho do trecho do texto
        /// </summary>
        /// <param name="row">Coordenada no eixo Y</param>
        /// <param name="column">Coordenada no eixo X</param>
        /// <param name="length">Tamanho do trecho a ser recuperado</param>
        /// <returns>Texto encontrado</returns>
        public string GetScreen(int row, int column, int length)
        {
            return objXMFDisplay.Call<string>("GetScreen2", row, column, length).Replace("\0", " ");
        }

        /// <summary>
        /// Insere um texto a partir de uma determinada coordenada na tela corrente
        /// </summary>
        /// <param name="text">Texto a ser inserido</param>
        /// <param name="row">Coordenada no eixo Y</param>
        /// <param name="column">Coordenada no eixo X</param>
        public void PutScreen(string text, int row, int column)
        {
            objXMFDisplay.Call("PutScreen2", text, row, column);
            SetLastScreen();
        }

        /// <summary>
        /// Insere uma data na tela corrente a partir de uma coordenada
        /// </summary>
        /// <param name="data">Data a ser inserida</param>
        /// <param name="row">Coordenada no eixo Y</param>
        /// <param name="column">Coordenada no eixo X</param>
        public void PutScreen(DateTime data, int row, int column)
        {
            objXMFDisplay.Call("PutScreen2", data.Day.ToString("00"), row, column);
            objXMFDisplay.Call("PutScreen2", data.Month.ToString("00"), row, column + 5);
            objXMFDisplay.Call("PutScreen2", data.Year.ToString("0000").Substring(2, 2), row, column + 10);
            SetLastScreen();
        }

        /// <summary>
        /// Limpa um determinado espaço na tela antes de inserir um novo texto
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="size"></param>
        public void PutScreen(string text, int row, int column, int size)
        {
            objXMFDisplay.Call("PutScreen2", new String(' ', size), row, column);
            PutScreen(text, row, column);
        }


        /// <summary>
        /// Insere um texto na tela corrente a partir do próximo espaço editável disponível
        /// </summary>
        /// <param name="text">Texto a ser inserido</param>
        public void PutScreen(string text)
        {
            objXMFDisplay.Call("SendKeys", text);
            SetLastScreen();
        }

        /// <summary>
        /// Envia uma tecla para o emulador
        /// </summary>
        /// <param name="key">Tecla a ser enviada</param>
        public void SendKey(ConsoleKey key)
        {
            SetLastScreen();
            objXMFDisplay.Call("SendKeys", key.ToComandText());
        }

        /// <summary>
        /// Verifica se um determinado texto se encontra em uma posição durante um período de tempo com base nas coordenadas configuradas em uma chave do arquivo de configurações do emulador de terminal mainframe
        /// </summary>
        /// <param name="chaveTexto">Texto a ser procurado</param>
        /// <param name="chavePosicao">Chave das coordenadas no arquivo de configurações do emulador de terminal</param>
        /// <param name="mensagemErro">Mensagem de erro que deve ser retornada na exceção caso a tela não encontre o texto procurado</param>
        public void ValidaPassoOk(string chaveTexto, string chavePosicao, string mensagemErro)
        {
            ValidaPassoOk(chaveTexto, chavePosicao, mensagemErro, 0);
        }

        /// <summary>
        /// Verifica se um determinado texto se encontra em uma posição durante um período de tempo com base nas coordenadas configuradas em uma chave do arquivo de configurações do emulador de terminal mainframe 
        /// </summary>
        /// <param name="chaveTexto">Texto a ser procurado</param>
        /// <param name="chavePosicao">Chave das coordenadas no arquivo de configurações do emulador de terminal</param>
        /// <param name="mensagemErro">Mensagem de erro que deve ser retornada na exceção caso a tela não encontre o texto procurado</param>
        /// <param name="timeout">Tempo de espera em milisegundos</param>
        public void ValidaPassoOk(string chaveTexto, string chavePosicao, string mensagemErro, int timeout)
        {
            if (!VerificaPassoAtual(chaveTexto, chavePosicao, timeout))
            {
                throw new UnexpectedScreenException(mensagemErro, this.GetScreen());
            }
        }

        public string GetEditable()
        {
            int row;
            int column;
            try
            {
                for (row = 1; row <= 24; row++)
                {
                    for (column = 1; column < 80; column++)
                    {
                        PutScreen("@", row, column);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return GetScreen().Replace("\t", " ");
        }


        public bool VerificaPassoAtual(string chaveTexto, string chavePosicao, int timeout)
        {
            if (TerminalConfig.Settings.AllKeys.Contains(chaveTexto))
            {
                Point posicao = TerminalConfig.GetPositionSetting(chavePosicao);
                SetStart();
                while (!CheckTimeout(timeout) && !this.CompareText(TerminalConfig.Settings[chaveTexto].Value, posicao.Y, posicao.X))
                {
                    Sleep(SLEEP);
                    Application.DoEvents();
                }
                if (!this.CompareText(TerminalConfig.Settings[chaveTexto].Value, posicao.Y, posicao.X))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
