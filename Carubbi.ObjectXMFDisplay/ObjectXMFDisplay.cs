using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Carubbi.ObjectXMFDisplay.Config;
using Carubbi.ObjectXMFDisplay.Util;
using Carubbi.Utils.Data;
using Carubbi.Utils.Persistence;
using Carubbi.Utils.UIControls;
namespace Carubbi.ObjectXMFDisplay
{
    /// <summary>
    /// Controle Simulador de Emulação de Terminal Mainframe baseado na interface do componente Microfocus Rumba OnWeb
    /// </summary>
    public partial class AxObjectXMFDisplay : UserControl, System.ComponentModel.ISupportInitialize
    {
        private Size BASE_SIZE = new Size { Width = 480, Height = 320 };
        private bool _connected = false;
        private Tela _telaAtual = new Tela();
        private Point _cursorPosition = new Point { X = 0, Y = 0 };
        private BaseMF _configuracoes = new BaseMF();

        public AxObjectXMFDisplay()
        {
            InitializeComponent();
           
        }

        #region ISupportInitialize
        public void BeginInit() {}
        public void EndInit() {}
        #endregion

        #region Variáveis privadas dos parâmetros
        private bool _autoFont;
        private int _autoFontMinimumWidth;
        private bool _characterCase;
        private string _characterSetId2;
        private int _clipboardConfiguration;
        private int _cursorBlinkRate;
        private int _cursorColumn;
        private int _cursorRow;
        private int _cursorSize;
        private int _cursorStyle;
        private bool _dblClkSelection;
        private bool _disableSoundAlarm;
        private int _disableStartPrinter;
        private int _eventVersion;
        private int _fixedAspectRatio;
        private bool _fontBold;
        private int _fontCharacterSpacing;
        private bool _fontItalic;
        private int _fontSize;
        private string _fontTypeFace;
        private int _fontWeight;
        private int _GDDMLogicalHeight;
        private int _GDDMLogicalWidth;
        private int _hostInterfaceConfiguration;
        private int _keyboardConfiguration;
        private bool _keyboardUnlockWCC;
        private int _licenseScheme;
        private string _lightPen;
        private bool _numericFieldOverride;
        private AxHost.State _ocxState;
        private bool _outlinePresentationSpace;
        private bool _padFieldsWithSpaces;
        private int _pasteInsert;
        private bool _RDE_Trace_Hsynch;
        private bool _reportInformation;
        private int _screenIdVersion;
        private bool _showLightPenCursor;
        private bool _smartInsert;
        private int _textBlinkRate;
        private int _watermarkConfiguration;
        private bool _wordWrap;
        #endregion

        #region Eventos

        public delegate void AfterConnectDelegate(object sender, EventArgs e);

        private void AxObjectXMFDisplay_Resize(object sender, EventArgs e)
        {
            
            float zoomFactorWidth = (float)outputView.Width / (float)BASE_SIZE.Width;
            float zoomFactorHeight = (float)outputView.Height / (float)BASE_SIZE.Height;
            float zoomFactor = (zoomFactorWidth > zoomFactorHeight ? zoomFactorHeight : zoomFactorWidth);
            outputView.ZoomFactor = zoomFactor > 0 ? zoomFactor : outputView.ZoomFactor;
        }
        #endregion

        #region Propriedades
        public bool HostConnected { get { return _connected; } }
        public override string Text { get { return _telaAtual.Texto; } }
        public string[] MFText { get { return _telaAtual.Texto.ToLineArray(); } }

        protected bool IsInDesignMode
        {
            get
            {
                return ((bool)Parent.GetType().GetProperty("IsInDesignMode").GetValue(Parent, null)) || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }

        #endregion

        #region Métodos Públicos

        public void Connect()
        {
            _connected = true;
        }

        public void Disconnect()
        {
            Text = "";
            _connected = false;
        }

        public string GetScreen2(int row, int column, int length)
        {
            row--;   
            column--;
            StringBuilder stbScreen = new StringBuilder();
            while (length > 0)
            {
                stbScreen.AppendFormat(MFText[row++].Substring(column, length>80? 80: length));
                length = length - 80;
            }
            return stbScreen.ToString();
        }

        public void PutScreen2(string text, int row, int column)
        {
            SetPosition(column, row);
            PutScreen(text);
        }

        public void RDE_SendKeys_Hsynch(string command, bool waitForEvents, int waitCount, int timeOut, string szVerString, int row, int column)
        {
            KeyEvent(command);
        }

        public void SendKeys(string text)
        {
            if (!KeyEvent(text))
            {
                PutScreen2(text, _cursorPosition.Y, _cursorPosition.X);
            }
        }

        public void LoadConfig(string configName)
        {
            if (!this.IsInDesignMode)
            {
                _configuracoes.Nome = configName;

                //Verifica se já não existe um config Salvo
                if (!File.Exists(String.Format("{0}\\{1}.xml", Application.LocalUserAppDataPath, _configuracoes.Nome)))
                {
                    TelaConfiguracoes config = new TelaConfiguracoes() { Configuracoes = _configuracoes };
 
                    if (ParentForm == null)
                        config.ShowDialog(new Form());
                    else
                        config.ShowDialog(ParentForm);
                }
                else
                {
                    try
                    {
                        var serializer = new Serializer<BaseMF>();
                        _configuracoes = serializer.XmlDeserialize(File.ReadAllText(String.Format("{0}\\{1}.xml", Application.LocalUserAppDataPath, _configuracoes.Nome)));
                    }
                    catch
                    {
                        MessageBox.Show("O arquivo de conficurações salvo é inválido ou está corrompido.\nPor favor, selecione um novo arquivo.");
                        TelaConfiguracoes config = new TelaConfiguracoes() { Configuracoes = _configuracoes };
                        config.ShowDialog(ParentForm);
                    }
                }
                _telaAtual = new Tela(_configuracoes.TelaInicial, _configuracoes);
                UpdateScreen();
            }
        }
        #endregion

        #region Métodos Privados
        private bool KeyEvent(string command)
        {
            
            try
            {
                Tela tela = _telaAtual.NavegaConsoleKey(command.ToConsoleKey(), _cursorPosition);
                if (tela.Texto != "")
                {
                    _telaAtual = tela;
                }
                UpdateScreen();
                return true;
            }
            catch (ArgumentException)
            { 
                return false;
            }
        }

        
        
        private void UpdateScreen()
        {
              outputView.InvokeIfRequired(c => c.Text = _telaAtual.Texto);
        }

        private void SetPosition(int positionX, int positionY)
        {
            _cursorPosition.X = positionX;
            _cursorPosition.Y = positionY;
            _telaAtual.PosicaoX = positionX;
            _telaAtual.PosicaoY = positionY;
        }

        private void PutScreen(string text)
        {
            _telaAtual.Escreve(text);
            UpdateScreen();
        }
        #endregion

        #region Atributos do controle
        public bool AutoFont { get { return _autoFont; } set { _autoFont = value; } }
        public int AutoFontMinimumWidth { get { return _autoFontMinimumWidth; } set { _autoFontMinimumWidth = value; } }
        public bool CharacterCase { get { return _characterCase; } set { _characterCase = value; } }
        public string CharacterSetId2 { get { return _characterSetId2; } set { _characterSetId2 = value; } }
        public int ClipboardConfiguration { get { return _clipboardConfiguration; } set { _clipboardConfiguration = value; } }
        public int CursorBlinkRate { get { return _cursorBlinkRate; } set { _cursorBlinkRate = value; } }
        public int CursorColumn { get { return _cursorColumn; } set { _cursorPosition.X = value; _cursorColumn = value; } }
        public int CursorRow { get { return _cursorRow; } set { _cursorPosition.Y = value; _cursorRow = value; } }
        public int CursorSize { get { return _cursorSize; } set { _cursorSize = value; } }
        public int CursorStyle { get { return _cursorStyle; } set { _cursorStyle = value; } }
        public bool DblClkSelection { get { return _dblClkSelection; } set { _dblClkSelection = value; } }
        public bool DisableSoundAlarm { get { return _disableSoundAlarm; } set { _disableSoundAlarm = value; } }
        public int DisableStartPrinter { get { return _disableStartPrinter; } set { _disableStartPrinter = value; } }
        public int EventVersion { get { return _eventVersion; } set { _eventVersion = value; } }
        public int FixedAspectRatio { get { return _fixedAspectRatio; } set { _fixedAspectRatio = value; } }
        public bool FontBold { get { return _fontBold; } set { _fontBold = value; } }
        public int FontCharacterSpacing { get { return _fontCharacterSpacing; } set { _fontCharacterSpacing = value; } }
        public bool FontItalic { get { return _fontItalic; } set { _fontItalic = value; } }
        public int FontSize { get { return _fontSize; } set { _fontSize = value; } }
        public string FontTypeFace { get { return _fontTypeFace; } set { _fontTypeFace = value; } }
        public int FontWeight { get { return _fontWeight; } set { _fontWeight = value; } }
        public int GDDMLogicalHeight { get { return _GDDMLogicalHeight; } set { _GDDMLogicalHeight = value; } }
        public int GDDMLogicalWidth { get { return _GDDMLogicalWidth; } set { _GDDMLogicalWidth = value; } }
        public int HostInterfaceConfiguration { get { return _hostInterfaceConfiguration; } set { _hostInterfaceConfiguration = value; } }
        public int KeyboardConfiguration { get { return _keyboardConfiguration; } set { _keyboardConfiguration = value; } }
        public bool KeyboardUnlockWCC { get { return _keyboardUnlockWCC; } set { _keyboardUnlockWCC = value; } }
        public int LicenseScheme { get { return _licenseScheme; } set { _licenseScheme = value; } }
        public string LightPen { get { return _lightPen; } set { _lightPen = value; } }
        public bool NumericFieldOverride { get { return _numericFieldOverride; } set { _numericFieldOverride = value; } }
        public AxHost.State OcxState { get { return _ocxState; } set { _ocxState = value; } }
        public bool OutlinePresentationSpace { get { return _outlinePresentationSpace; } set { _outlinePresentationSpace = value; } }
        public bool PadFieldsWithSpaces { get { return _padFieldsWithSpaces; } set { _padFieldsWithSpaces = value; } }
        public int PasteInsert { get { return _pasteInsert; } set { _pasteInsert = value; } }
        public bool RDE_Trace_Hsynch { get { return _RDE_Trace_Hsynch; } set { _RDE_Trace_Hsynch = value; } }
        public bool ReportInformation { get { return _reportInformation; } set { _reportInformation = value; } }
        public int ScreenIdVersion { get { return _screenIdVersion; } set { _screenIdVersion = value; } }
        public bool ShowLightPenCursor { get { return _showLightPenCursor; } set { _showLightPenCursor = value; } }
        public bool SmartInsert { get { return _smartInsert; } set { _smartInsert = value; } }
        public int TextBlinkRate { get { return _textBlinkRate; } set { _textBlinkRate = value; } }
        public int WatermarkConfiguration { get { return _watermarkConfiguration; } set { _watermarkConfiguration = value; } }
        public bool WordWrap { get { return _wordWrap; } set { _wordWrap = value; } }
        #endregion
    }
}
