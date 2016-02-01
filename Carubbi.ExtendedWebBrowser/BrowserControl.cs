using System;
using System.Drawing;
using System.Windows.Forms;

namespace Carubbi.ExtendedWebBrowser
{
    public partial class BrowserControl : UserControl
    {
        private ExtendedWebBrowser _browser;

        // Updates the addres box with the actual URL of the document
        private void UpdateAddressBox()
        {
            string urlString = this.WebBrowser.Document.Url.ToString();
            if (!urlString.Equals(this.addressTextBox.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                this.addressTextBox.Text = urlString;
            }
        }

        // Used for the go button
        private void goButton_Click(object sender, EventArgs e)
        {
            Navigate();
        }

        // Navigate to the typed address
        private void Navigate()
        {
            this.WebBrowser.Navigate(this.addressTextBox.Text);
        }

        // Used for obtaining the MainForm from a control
        private static Form GetMainFormFromControl(Control control)
        {
            while (control != null)
            {
                if (control is Form)
                    break;
                control = control.Parent;
            }
            return control as Form;
        }

        // Used for catching the Enter key in the textbox
        private void addressTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                Navigate();
            }
        }

        protected void _browser_StartNewWindow(object sender, BrowserExtendedNavigatingEventArgs e)
        {
            // Here we do the pop-up blocker work

            // Note that in Windows 2000 or lower this event will fire, but the
            // event arguments will not contain any useful information
            // for blocking pop-ups.

            // There are 4 filter levels.
            // None: Allow all pop-ups
            // Low: Allow pop-ups from secure sites
            // Medium: Block most pop-ups
            // High: Block all pop-ups (Use Ctrl to override)

            // We need the instance of the main form, because this holds the instance
            // to the WindowManager.
            Form mf = GetMainFormFromControl(sender as Control);
            if (mf == null)
                return;

            // Allow a popup when there is no information available or when the Ctrl key is pressed
            bool allowPopup = (e.NavigationContext == UrlContext.None) || ((e.NavigationContext & UrlContext.OverrideKey) == UrlContext.OverrideKey);

            if (!allowPopup)
            {
                // Give None, Low & Medium still a chance.
                switch (FilterLevel)
                {
                    case PopupBlockerFilterLevel.None:
                        allowPopup = true;
                        break;
                    case PopupBlockerFilterLevel.Low:
                        // See if this is a secure site
                        if (this.WebBrowser.EncryptionLevel != WebBrowserEncryptionLevel.Insecure)
                            allowPopup = true;
                        else
                            // Not a secure site, handle this like the medium filter
                            goto case PopupBlockerFilterLevel.Medium;
                        break;
                    case PopupBlockerFilterLevel.Medium:
                        // This is the most dificult one.
                        // Only when the user first inited and the new window is user inited
                        if ((e.NavigationContext & UrlContext.UserFirstInited) == UrlContext.UserFirstInited && (e.NavigationContext & UrlContext.UserInited) == UrlContext.UserInited)
                            allowPopup = true;
                        break;
                }
            }

            if (allowPopup)
            {
                // Check wheter it's a HTML dialog box. If so, allow the popup but do not open a new tab
                if (!((e.NavigationContext & UrlContext.HtmlDialog) == UrlContext.HtmlDialog))
                {
                    ExtendedWebBrowser ewb = (mf as IWindowManaged).WindowManager.New(false, "Popup");
                    // The (in)famous application object
                    e.AutomationObject = ewb.Application;
                }
            }
            else
                // Here you could notify the user that the pop-up was blocked
                e.Cancel = true;

        }

        protected void _browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            UpdateAddressBox();
        }

        protected void _browser_DownloadComplete(object sender, EventArgs e)
        {
            // Check wheter the document is available (it should be)
            if (this.WebBrowser.Document != null)
            {
                // Subscribe to the Error event
                this.WebBrowser.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
                UpdateAddressBox();
            }
        }

        protected void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            // We got a script error, record it
            ScriptErrorManager.Instance.ErrorForm = this.ErrorForm;
            ScriptErrorManager.Instance.RegisterScriptError(e.Url, e.Description, e.LineNumber);
            // Let the browser know we handled this error.
            e.Handled = true;
        }

        protected void _browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            UpdateAddressBox();
            this.WebBrowser.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
        }

        public PopupBlockerFilterLevel FilterLevel { get; set; }

        public Form ErrorForm { get; set; }

        public BrowserControl(string name)
        {
            InitializeComponent();
            _browser = new ExtendedWebBrowser(name);
            _browser.Dock = DockStyle.Fill;
            _browser.DownloadComplete += new EventHandler(_browser_DownloadComplete);
            _browser.Navigated += new WebBrowserNavigatedEventHandler(_browser_Navigated);
            _browser.StartNewWindow += new StartNewWindowHandler(_browser_StartNewWindow);
            _browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_browser_DocumentCompleted);
            this.containerPanel.Controls.Add(_browser);

            // Make the magenta color transparent on the go button
            Bitmap bmp = (Bitmap)goButton.Image;
            bmp.MakeTransparent(Color.Magenta);
        }

        /// <summary>
        /// Permite que outro código obtenha uma referencia ao Componente ExtendedWebBrowser 
        /// </summary>
        public ExtendedWebBrowser WebBrowser
        {
            get { return _browser; }
        }
    }

    public delegate void StartNewWindowHandler(object sender, BrowserExtendedNavigatingEventArgs e);
}
