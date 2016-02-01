using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

namespace Carubbi.JavascriptWatcher
{
    public partial class JavascriptWatcher
    {
        /// <summary>
        /// Classe com os métodos de comunicação entre o javascript e o .net
        /// </summary>
        [ComVisible(true)]
        public class ScriptInterface : WebBrowser
        {
            

            private JavascriptWatcher _context;

            private WebBrowser _webBrowser;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public bool InterceptConfirm(string text)
            {
                if (_context.ConfirmIntercepted != null)
                {
                    var ea = new ConfirmInterceptedEventArgs() { Text = text };
                    _context.ConfirmIntercepted(_webBrowser, ea);
                    return ea.Result;
                }
                return false;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <param name="webBrowser"></param>
            public ScriptInterface(JavascriptWatcher context, WebBrowser webBrowser)
            {
                _context = context;
                _webBrowser = webBrowser;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="windowToBeOpened"></param>
            public void InterceptWindowOpen(string windowToBeOpened)
            {
                if (_context.WindowOpenIntercepted != null)
                {
                    if (string.IsNullOrEmpty(windowToBeOpened))
                    {
                        windowToBeOpened = "URL não informada";
                    }
                    Uri uriToOpen = null;
                    try
                    {
                        uriToOpen = new Uri(windowToBeOpened);
                    }
                    catch
                    {
                        uriToOpen= new Uri(windowToBeOpened, UriKind.Relative);
                    }
                    _context.WindowOpenIntercepted(_webBrowser, new WindowOpenInterceptedEventArgs() { Url = new Uri(windowToBeOpened, UriKind.Relative) });
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="text"></param>
            public void InterceptAlerts(string text)
            {
                if (_context.AlertIntercepted != null)
                {
                    _context.AlertIntercepted(_webBrowser, new AlertInterceptedEventArgs() { AlertText = text });
                }
            }
        }
    }
}
