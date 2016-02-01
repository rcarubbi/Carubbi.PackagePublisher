using System;
using System.Windows.Forms;
using mshtml;

namespace Carubbi.JavascriptWatcher
{
    /// <summary>
    /// Monitor de eventos javascript para controle e automação de páginas web
    /// </summary>
    public partial class JavascriptWatcher
    {
        private WebBrowser _context;

        private bool _supressAlert;
        private bool _supressWindowOpen;

        private void _context_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            AttachHandlers();
        }

        private void _context_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            AttachHandlers();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public JavascriptWatcher(WebBrowser context)
        {
            _context = context;
            
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AlertInterceptedEventArgs> AlertIntercepted;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<WindowOpenInterceptedEventArgs> WindowOpenIntercepted;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ConfirmInterceptedEventArgs> ConfirmIntercepted;

        /// <summary>
        /// Interrompe o monitoramento da página
        /// </summary>
        public void Stop()
        {
            if (_context != null)
            {
                _context.Navigated -= _context_Navigated;
                _context.DocumentCompleted -= _context_DocumentCompleted;
            }
        }

        /// <summary>
        /// Inicia o monitoramento da página
        /// </summary>
        /// <param name="supressAlert">Indica se comandos alert(); devem ser suprimidos ou devem seguir a implementação padrão do navegador</param>
        /// <param name="supressWindowOpen">Indica se comandos windows.open(); devem ser suprimidos ou devem seguir a implementação padrão do navegador</param>
        public void Start(bool supressAlert, bool supressWindowOpen)
        {
            Start(supressAlert, supressWindowOpen, false);
        }

        /// <summary>
        /// Inicia o monitoramento da página
        /// </summary>
        /// <param name="supressAlert">Indica se comandos alert(); devem ser suprimidos e controlados via código ou devem seguir a implementação padrão do navegador</param>
        /// <param name="supressWindowOpen">Indica se comandos windows.open(); devem ser suprimidos e controlados via código ou devem seguir a implementação padrão do navegador</param>
        /// <param name="attachHandlersInstantly">Indica se o monitoramento deve iniciar imediatamente ou apenas após o próximo documento ser solicitado e ter seu carregamento completo</param>
        public void Start(bool supressAlert, bool supressWindowOpen, bool attachHandlersInstantly)
        {
            _context.Navigated += new WebBrowserNavigatedEventHandler(_context_Navigated);
            _context.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_context_DocumentCompleted);
            if (attachHandlersInstantly)
            {
                AttachHandlers();
            }
            _supressAlert = supressAlert;
            _supressWindowOpen = supressWindowOpen;
        }

        private void InvokeIfRequired(Control c, Action action)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Insere o código Javascript na página corrente que permite a interceptação dos comandos alert, window.open e confirm pela aplicação .net
        /// </summary>
        public void AttachHandlers()
        {
            InvokeIfRequired(_context, AttachHandlerImpl);
        }

        
        private void AttachHandlerImpl()
        {
            var scriptInterface = new ScriptInterface(this, _context);

            _context.ObjectForScripting = scriptInterface;
            string js = @"window.alertProxy = window.alert;
                          window.windowOpenProxy = window.open;
                          window.confirmProxy = window.confirm;
                          
                          var customConfirm = function() {
                                return window.external.InterceptConfirm(arguments[0]);
                          }

                          var customWindowOpen = function() {
                             window.external.InterceptWindowOpen(arguments[0]);
                             " + (!_supressWindowOpen
                              ? @"return window.windowOpenProxy(arguments[0], arguments[1], arguments[2]);"
                              : @"") + @"
                          };
                          var customAlert = function() {
                                window.external.InterceptAlerts(arguments[0]);" +
                                (!_supressAlert
                                ? @"return window.alertProxy(arguments[0]);"
                                : @"return false;") +
                          @" };
                                
                                window.alert = customAlert;
                                window.open = customWindowOpen;
                                window.confirm = customConfirm;
                                var frameCount = window.frames.length, x = 0;
                                for (;x < frameCount; x++) 
                                {
                                    window.frames[x].confirm = customConfirm;
                                    window.frames[x].open = customWindowOpen;
                                    window.frames[x].alert = customAlert;
                                };";

            HtmlElement el = _context.Document.CreateElement("script");
            IHTMLScriptElement dom = (IHTMLScriptElement)el.DomElement;
            dom.text = js;
            if (_context.Document.Body != null)
            {
                _context.Document.Body.AppendChild(el);
            }


            HtmlElement jqueryEl = _context.Document.CreateElement("script");
            IHTMLScriptElement domJquery = (IHTMLScriptElement)el.DomElement;
            dom.src = "//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js";
            if (_context.Document.Body != null)
            {
                _context.Document.Body.AppendChild(jqueryEl);
            }
        }
    }
}
