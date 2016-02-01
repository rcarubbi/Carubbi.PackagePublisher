using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Carubbi.JavascriptWatcher;
using Carubbi.Utils.UIControls;
using Microsoft.Win32;
using mshtml;
namespace Carubbi.Web.Utils
{
    /// <summary>
    /// Estrutura de dados com opções para manipulação da página html
    /// </summary>
    internal class WebBrowserParams
    {
        /// <summary>
        /// Quantidade de Postbacks encontrados
        /// </summary>
        internal int PostbackCounts { get; set; }

     
        /// <summary>
        /// Textos encontrados em Alerts
        /// </summary>
        internal StringCollection MonitoredAlertTexts { get; set; }

        /// <summary>
        /// Objeto responsável por monitorar de eventos javascript em uma página
        /// </summary>
        internal Carubbi.JavascriptWatcher.JavascriptWatcher jsWatcher { get; set; }

        /// <summary>
        /// Indica se um alert foi disparado
        /// </summary>
        public bool AlertRaised { get; set; }

        /// <summary>
        /// Texto do alert disparado
        /// </summary>
        public string AlertRaisedText { get; set; }


        /// <summary>
        /// Url do window.open disparado
        /// </summary>
        public string WindowOpenUrl { get; set; }
    }


    /// <summary>
    /// Extension Methods da classe WebBrowser
    /// </summary>
    public static class WebBrowserExtensions
    {
        private static Dictionary<string, WebBrowserParams> webBrowserInstances = new Dictionary<string, WebBrowserParams>();

        /// <summary>
        /// Recupera a quantidade corrente de postbacks efetuados no browser
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int GetPostbackCounter(this WebBrowser instance)
        {
            if (webBrowserInstances.ContainsKey(instance.Name))
                return webBrowserInstances[instance.Name].PostbackCounts;
            else
                return 0;
        }

        /// <summary>
        /// Inicia o processo de contar a quantidade de postbacks do browser para poder recuperar com o método WaitForPostbacks
        /// </summary>
        /// <param name="instance"></param>
        public static void StartMonitoringPostbacks(this WebBrowser instance)
        {
            instance.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(instance_DocumentCompletedWaitPostback);
        }

        internal static void instance_DocumentCompletedWaitPostback(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            if (webBrowserInstances.ContainsKey(((WebBrowser)sender).Name))
                webBrowserInstances[((WebBrowser)sender).Name].PostbackCounts++;
            else
            {
                webBrowserInstances.Add(((WebBrowser)sender).Name, new WebBrowserParams());
            }
        }

        /// <summary>
        /// Inicia o processo de monitoramento de alerts no browser
        /// </summary>
        /// <param name="instance"></param>
        public static void StartMonitoringJavascript(this WebBrowser instance, bool supressAlert, bool supressWindowOpen)
        {
            if (!webBrowserInstances.ContainsKey(instance.Name))
            {
                webBrowserInstances.Add(instance.Name, new WebBrowserParams());
                webBrowserInstances[instance.Name].MonitoredAlertTexts = new StringCollection();
                webBrowserInstances[instance.Name].WindowOpenUrl = string.Empty;
            }
            if (webBrowserInstances[instance.Name] == null)
            {
                webBrowserInstances[instance.Name] = new WebBrowserParams();
                webBrowserInstances[instance.Name].MonitoredAlertTexts = new StringCollection();
            }
            webBrowserInstances[instance.Name].jsWatcher = new Carubbi.JavascriptWatcher.JavascriptWatcher(instance);
            webBrowserInstances[instance.Name].jsWatcher.Start(supressAlert, supressWindowOpen);
            webBrowserInstances[instance.Name].jsWatcher.AlertIntercepted += jsWatcher_AlertIntercepted;
            webBrowserInstances[instance.Name].jsWatcher.WindowOpenIntercepted += jsWatcher_WindowOpenIntercepted;

        }

        static void jsWatcher_WindowOpenIntercepted(object sender, WindowOpenInterceptedEventArgs e)
        {
            var webBrowser = (WebBrowser)sender;
            webBrowserInstances[webBrowser.Name].WindowOpenUrl = e.Url.ToString();
        }

        static void jsWatcher_AlertIntercepted(object sender, AlertInterceptedEventArgs e)
        {
            var webBrowser = (WebBrowser)sender;
            webBrowserInstances[webBrowser.Name].MonitoredAlertTexts.Add(e.AlertText);
            webBrowserInstances[webBrowser.Name].AlertRaised = true;
            webBrowserInstances[webBrowser.Name].AlertRaisedText = e.AlertText;
        }

        /// <summary>
        /// Ignora apenas alerts de erro de script
        /// </summary>
        /// <param name="instance"></param>
        public static void SuppressScriptErrorsOnly(this WebBrowser instance, bool supressByDefault)
        {
            // Ensure that ScriptErrorsSuppressed is set to false.
            instance.ScriptErrorsSuppressed = supressByDefault;

            // Handle DocumentCompleted to gain access to the Document object.
            instance.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);
        }

        private static void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).InvokeIfRequired(i => (i as WebBrowser).Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error));
        }

        private static void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            // Ignore the error and suppress the error dialog box. 
            e.Handled = true;
        }

        /// <summary>
        /// Aguarda por alert ou postback por um determinado tempo
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>ResponseType indica se foi lançado alert ou postback, responseText indica o texto do alert</returns>
        public static ResponseResult WaitResponse(this WebBrowser instance, int timeout)
        {
            Stopwatch cronometer = new Stopwatch();
            cronometer.Start();

            if (!webBrowserInstances.ContainsKey(instance.Name))
            {
                webBrowserInstances.Add(instance.Name, new WebBrowserParams());
            }

            while (instance.GetPostbackCounter() < 1
                && !webBrowserInstances[instance.Name].AlertRaised
                && string.IsNullOrEmpty(webBrowserInstances[instance.Name].WindowOpenUrl)
                && cronometer.ElapsedMilliseconds < timeout)
            {
                Application.DoEvents();
            }
            cronometer.Stop();

            ResponseResult result = new ResponseResult();

            if (cronometer.ElapsedMilliseconds >= timeout)
            {
                result.ResponseType = ResponseType.Timeout;
            }
            else if (!webBrowserInstances[instance.Name].AlertRaised && string.IsNullOrEmpty(webBrowserInstances[instance.Name].WindowOpenUrl))
            {
                if (instance != null && webBrowserInstances[instance.Name] != null)
                {
                    webBrowserInstances[instance.Name].PostbackCounts = 0;
                    result.ResponseType = ResponseType.Postback;
                }
                else
                {
                    result.ResponseType = ResponseType.Unknown;
                }
            }
            else if (webBrowserInstances[instance.Name].AlertRaised)
            {
                result.ResponseType = ResponseType.Alert;
                result.ResponseText = webBrowserInstances[instance.Name].AlertRaisedText;
                webBrowserInstances[instance.Name].AlertRaisedText = string.Empty;
                webBrowserInstances[instance.Name].AlertRaised = false;
            }
            else if (!string.IsNullOrEmpty(webBrowserInstances[instance.Name].WindowOpenUrl))
            {
                result.ResponseType = ResponseType.WindowOpen;
                result.ResponseText = webBrowserInstances[instance.Name].WindowOpenUrl;
                webBrowserInstances[instance.Name].WindowOpenUrl = string.Empty;
            }
            else
            {
                result.ResponseType = ResponseType.Unknown;
            }


            return result;
        }

        /// <summary>
        /// Prepara o webbrowser para monitorar postbacks, alerts e suprimir erros de script
        /// </summary>
        /// <param name="instance"></param>
        public static void Initialize(this WebBrowser instance, bool supressAlert, bool supressWindowOpen, bool supressByDefault)
        {
            instance.StartMonitoringPostbacks();
            instance.StartMonitoringJavascript(supressAlert, supressWindowOpen);
            instance.SuppressScriptErrorsOnly(supressByDefault);
            instance.SupressCertificateErrors();
        }

        /// <summary>
        /// Descarta o objeto WebBrowser e seus dependentes
        /// </summary>
        /// <param name="instance"></param>
        public static void DisposeBrowser(this WebBrowser instance)
        {
            if (webBrowserInstances.ContainsKey(instance.Name))
            {
                instance.StopMonitoringJavascript();
                if (webBrowserInstances[instance.Name] != null)
                {
                    if (webBrowserInstances[instance.Name].MonitoredAlertTexts != null)
                    {
                        webBrowserInstances[instance.Name].MonitoredAlertTexts.Clear();
                        webBrowserInstances[instance.Name].MonitoredAlertTexts = null;
                    }
                }
                webBrowserInstances.Remove(instance.Name);
                webBrowserInstances[instance.Name] = null;
            }
         
            instance.Dispose();
        }


        /// <summary>
        /// Limpa variáveis temporárias utilizadas no monitoramento da página
        /// </summary>
        public static void ClearWebBrowserCache()
        {
            foreach (var entry in webBrowserInstances)
            {
                if (entry.Value != null)
                {
                    if (entry.Value.jsWatcher != null)
                    {
                        entry.Value.jsWatcher.Stop();
                        entry.Value.jsWatcher.AlertIntercepted -= jsWatcher_AlertIntercepted;
                        entry.Value.jsWatcher.WindowOpenIntercepted -= jsWatcher_WindowOpenIntercepted;
                        entry.Value.jsWatcher = null;
                    }

                    if (entry.Value.MonitoredAlertTexts != null)
                    {
                        entry.Value.MonitoredAlertTexts.Clear();
                        entry.Value.MonitoredAlertTexts = null;
                    }
                }
            }

            webBrowserInstances.Clear();
            webBrowserInstances = null;
            webBrowserInstances = new Dictionary<string, WebBrowserParams>();
        }

        /// <summary>
        /// Suprime erros de certificado
        /// </summary>
        /// <param name="instance"></param>
        public static void SupressCertificateErrors(this WebBrowser instance)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }


        /// <summary>
        /// Interrompe o monitoramento de Javascript na página
        /// </summary>
        /// <param name="instance"></param>
        public static void StopMonitoringJavascript(this WebBrowser instance)
        {
            if (webBrowserInstances.ContainsKey(instance.Name))
            {
                if (webBrowserInstances[instance.Name] != null && webBrowserInstances[instance.Name].jsWatcher != null)
                {
                    webBrowserInstances[instance.Name].jsWatcher.Stop();
                    webBrowserInstances[instance.Name].jsWatcher.AlertIntercepted -= jsWatcher_AlertIntercepted;
                    webBrowserInstances[instance.Name].jsWatcher.WindowOpenIntercepted -= jsWatcher_WindowOpenIntercepted;
                    webBrowserInstances[instance.Name].jsWatcher = null;
                }
            }
        }

        /// <summary>
        /// Inicializa um WebBrowser para ser controlado e automatizado
        /// </summary>
        /// <param name="instance"></param>
        public static void Initialize(this WebBrowser instance)
        {
            Initialize(instance, true, true, false);
        }


        private static Dictionary<string, string> MimeTypeToExtension = new Dictionary<string, string>();

        private static Dictionary<string, string> ExtensionToMimeType = new Dictionary<string, string>();


        /// <summary>
        /// Converte um mimetype em uma extensão de arquivo com base no registro do windows
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static string ConvertMimeTypeToExtension(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType.Trim()))
                throw new ArgumentNullException("mimeType");

            string key = string.Format(@"MIME\Database\Content Type\{0}", mimeType);
            string result;
            if (MimeTypeToExtension.TryGetValue(key, out result))
                return result;

            RegistryKey regKey;
            object value;

            regKey = Registry.ClassesRoot.OpenSubKey(key, false);
            value = regKey != null ? regKey.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            MimeTypeToExtension[key] = result;
            return result;
        }

        /// <summary>
        /// Recupera uma imagem do documento renderizado pelo WebBrowser e armazena em um bitmap
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Bitmap PrintScreen(this WebBrowser instance)
        {
            return PrintScreen(instance, 0);
        }

        /// <summary>
        /// Recupera uma imagem do documento renderizado pelo WebBrowser e armazena em um bitmap após um determinado período em milisegundos
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public static Bitmap PrintScreen(this WebBrowser instance, int waitTime)
        {
            DockStyle originalDock = DockStyle.None;

            var originalWidth = instance.Width;
            var originalHeight = instance.Height;
            Bitmap bitmap = null;

            instance.InvokeIfRequired(i =>
            {
                WebBrowser wb = (i as WebBrowser);
                try
                {
                    originalDock = wb.Dock;
                    wb.Dock = DockStyle.None;

                    if (originalWidth < 1200)
                    {
                        wb.Width = 1200;
                    }
                    else
                    {
                        wb.Width = originalWidth;
                    }

                    if (originalHeight < 900)
                    {
                        wb.Height = 900;
                    }
                    else
                    {
                        wb.Height = originalHeight;
                    }

                    wb.Parent.Focus();

                    bitmap = new Bitmap(wb.Width, wb.Height);

                    wb.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));

                    if (waitTime > 0)
                    {
                        wb.Focus();
                        Thread.Sleep(waitTime);
                    }
                    wb.Width = originalWidth;
                    wb.Height = originalHeight;
                }
                finally
                {
                    wb.Dock = originalDock;
                }
            });

            return bitmap;

        }


        /// <summary>
        /// Copia uma página através da área de transferência e a transforma em bitmap
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="htmlElementId"></param>
        /// <returns></returns>
        public static Bitmap PrintHtmlImage(this WebBrowser instance, string htmlElementId)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)instance.Document.DomDocument;
            IHTMLDocument3 doc3 = (IHTMLDocument3)instance.Document.DomDocument;
            IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();

            imgRange.add((IHTMLControlElement)doc3.getElementById(htmlElementId));

            imgRange.execCommand("Copy", false, null);

            Bitmap bmp = (Bitmap)Clipboard.GetDataObject().GetData(DataFormats.Bitmap);

            return bmp;
        }


        public static Bitmap PrintHtmlImage(this WebBrowser instance, HtmlElement element)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)instance.Document.DomDocument;
            IHTMLDocument3 doc3 = (IHTMLDocument3)instance.Document.DomDocument;
            IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();

            imgRange.add((IHTMLControlElement)element.DomElement);

            imgRange.execCommand("Copy", false, null);

            Bitmap bmp = (Bitmap)Clipboard.GetDataObject().GetData(DataFormats.Bitmap);

            return bmp;
        }
    }
}
