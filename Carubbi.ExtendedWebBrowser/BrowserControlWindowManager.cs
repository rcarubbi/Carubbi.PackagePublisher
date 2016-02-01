
using System;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;
using Carubbi.Utils.UIControls;
using Carubbi.Web.Utils;
namespace Carubbi.ExtendedWebBrowser
{
    /// <summary>
    /// Manages the tabs, and their contents
    /// </summary>
    public class BrowserControlWindowManager : IWindowManager
    {
        private TabControl _tabControl;

        private Form _errorForm;

        private void CheckCommandState()
        {
            BrowserCommands commands = BrowserCommands.None;
            if (ActiveBrowser != null)
            {
                if (ActiveBrowser.CanGoBack)
                    commands |= BrowserCommands.Back;
                if (ActiveBrowser.CanGoForward)
                    commands |= BrowserCommands.Forward;
                if (ActiveBrowser.IsBusy)
                    commands |= BrowserCommands.Stop;
                // Add the default commands
                // You could do some aditional checking here. 
                // For example, Home could be disabled if the user is allready on the home page
                commands |= BrowserCommands.Home;
                commands |= BrowserCommands.Search;
                commands |= BrowserCommands.Print;
                commands |= BrowserCommands.PrintPreview;
                commands |= BrowserCommands.Reload;
            }

            //// If it's not the active browser, return aswel..
            //if (extendedWebBrowser != ActiveBrowser)
            //  return;

            OnCommandStateChanged(new CommandStateEventArgs(commands));
        }

        private static BrowserControl BrowserControlFromBrowser(ExtendedWebBrowser browser)
        {
            // This is a little nasty. The Extended Web Browser is nested in 
            // a panel, wich is nested in the browser control

            // Since we want to avoid a NullReferenceException, do some checking

            // Check if we got a extended web browser
            if (browser == null)
                return null;

            // Check if it got a parent
            if (browser.Parent == null)
                return null;

            // Return the parent of the parent using a safe cast.
            return browser.Parent.Parent as BrowserControl;
        }

        #region Events that cause the status of toolbar buttons or menu items to change

        protected void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            CheckCommandState();
            ((WebBrowser)sender).Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
        }

        protected void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            // Ignore the error and suppress the error dialog box. 
            e.Handled = true;
        }

        protected void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            CheckCommandState();
        }

        protected void WebBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            CheckCommandState();
        }

        protected void WebBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            CheckCommandState();
        }

        protected void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckCommandState();
        }

        protected void WebBrowser_Quit(object sender, EventArgs e)
        {
            // This event is launched when window.close() is called from script
            ExtendedWebBrowser brw = sender as ExtendedWebBrowser;
            if (brw == null)
                return;
            // See which page it was on...
            BrowserControl bc = BrowserControlFromBrowser(brw);
            if (bc == null)
                return;

            TabPage page = bc.Tag as TabPage;
            if (page == null)
                return;

            // We got a page, remove & dispose it.
            _tabControl.InvokeIfRequired(tc => (tc as TabControl).TabPages.Remove(page));
            page.Dispose();

            var tabCount = 0;
            _tabControl.InvokeIfRequired(tc => tabCount = (tc as TabControl).TabPages.Count);
            if (tabCount == 0)
                _tabControl.InvokeIfRequired(tc => tc.Visible = false);
        }

        protected void WebBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            // Update the title of the tab page of the control.
            ExtendedWebBrowser ewb = sender as ExtendedWebBrowser;
            // Return if we got nothing (shouldn't happen)
            if (ewb == null) return;

            // This is a little nasty. The Extended Web Browser is nested in 
            // a panel, wich is nested in the browser control
            BrowserControl bc = BrowserControlFromBrowser(ewb);
            // If we got null, return
            if (bc == null) return;

            // The Tag of the BrowserControl should point to the TabPage
            TabPage page = bc.Tag as TabPage;
            // If not, return
            if (page == null) return;

            int tabIndex = _tabControl.TabPages.IndexOf(page);

            string documentTitle = GetTitleName(tabIndex) ?? ewb.DocumentTitle;

            if (documentTitle.Length > 30)
            {
                documentTitle = documentTitle.Substring(0, 30) + "...";
            }

            page.Text = documentTitle;
            // Set the full title as a tooltip
            page.ToolTipText = ewb.DocumentTitle;

        }

        protected void WebBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            // First, see if the active page is calling, or another page
            ExtendedWebBrowser ewb = sender as ExtendedWebBrowser;
            // Return if we got nothing (shouldn't happen)
            if (ewb == null) return;

            // This is a little nasty. The Extended Web Browser is nested in 
            // a panel, wich is nested in the browser control
            BrowserControl bc = BrowserControlFromBrowser(ewb);

            // The Tag of the BrowserControl should point to the TabPage
            TabPage page = bc.Tag as TabPage;
            // If not, return
            if (page == null) return;

            // See if 'page' is the active page
            TabPage selectedTab = null;
            _tabControl.InvokeIfRequired(tc => selectedTab = (tc as TabControl).SelectedTab);
            if (selectedTab == page)
            {
                // Yep, send the event that updates the status bar text
                OnStatusTextChanged(new TextChangedEventArgs(ewb.StatusText));
            }
        }

        protected virtual void OnCommandStateChanged(CommandStateEventArgs e)
        {
            if (CommandStateChanged != null)
                CommandStateChanged(this, e);
        }

        /// <summary>
        /// Raises the StatusTextChanged event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStatusTextChanged(TextChangedEventArgs e)
        {
            if (StatusTextChanged != null)
                StatusTextChanged(this, e);
        }


        #endregion

        public BrowserControlWindowManager(TabControl tabControl, Form errorForm)
        {
            _errorForm = errorForm;
            _tabControl = tabControl;
            _tabControl.InvokeIfRequired(tc => (tc as TabControl).SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged));
        }

        /// <summary>
        /// Closes the active tab
        /// </summary>
        public void Close(int index)
        {
            // Find the active page
            TabPage page = null;
            _tabControl.InvokeIfRequired(tc => page = (tc as TabControl).TabPages[index]);
            // Check wheter there is actually a page selected
            if (page != null)
            {
                _tabControl.InvokeIfRequired(tc =>
                    {
                        foreach (Control c in page.Controls)
                        {
                            if (c is Panel)
                            {
                                foreach (Control wb in c.Controls)
                                {
                                    (wb as WebBrowser).DisposeBrowser();
                                }
                            }
                        }
                        page.Controls.Clear();

                        // Remove the page
                        (tc as TabControl).TabPages.Remove(page);
                        // Dispose the page (controls on the page are also disposed this way)
                        page.Dispose();
                    });
            }


            _tabControl.InvokeIfRequired(tc =>
            {
                var tabControl = tc as TabControl;
                if (tabControl.TabPages.Count == 0)
                {
                    tabControl.Visible = false;
                }
            });
        }

        /// <summary>
        /// Opens a new browser tab, and navigates to the home page
        /// </summary>
        /// <returns>The instance of the browser created</returns>
        public ExtendedWebBrowser New(string title)
        {
            return New(true, title);
        }

        /// <summary>
        /// Opens a new browser tab
        /// </summary>
        /// <param name="navigateHome">true to immediately navigate to the homepage, otherwise false</param>
        /// <returns>The instance of the browser created</returns>
        // We cannot dispose the browsercontrol here
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public ExtendedWebBrowser New(bool navigateHome, string title)
        {

            ExtendedWebBrowser webBrowser = null;
            this._tabControl.InvokeIfRequired(tc =>
                {
                    TabControl tabControl = (tc as TabControl);
                    // Create a new tab page
                    TabPage page = new TabPage() { Text = title };
               
                    // Create a new browser control
                    BrowserControl browserControl = new BrowserControl(title);
                    browserControl.ErrorForm = _errorForm;
                    // Set the page as the Tag of the browser control, and vice-versa, this will come in handy later
                    browserControl.Tag = page;
                    page.Tag = browserControl;
                    // Dock the browser control
                    browserControl.Dock = DockStyle.Fill;
                    // Add the browser control to the tab page
                    page.Controls.Add(browserControl);
                    if (navigateHome)
                    {
                        // Navigate to the home page
                        browserControl.WebBrowser.GoHome();
                    }
                    // Wire some events
                    browserControl.WebBrowser.StatusTextChanged += new EventHandler(WebBrowser_StatusTextChanged);
                    browserControl.WebBrowser.DocumentTitleChanged += new EventHandler(WebBrowser_DocumentTitleChanged);
                    browserControl.WebBrowser.CanGoBackChanged += new EventHandler(WebBrowser_CanGoBackChanged);
                    browserControl.WebBrowser.CanGoForwardChanged += new EventHandler(WebBrowser_CanGoForwardChanged);
                    browserControl.WebBrowser.Navigated += new WebBrowserNavigatedEventHandler(WebBrowser_Navigated);
                    browserControl.WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                    browserControl.WebBrowser.Quit += new EventHandler(WebBrowser_Quit);
                  
                    // Add the new page to the tab control
                    tabControl.TabPages.Add(page);
                    tabControl.SelectedTab = page;
                    tabControl.Visible = true;

                    webBrowser = browserControl.WebBrowser;
                });


            return webBrowser;
        }

        public void Open(Uri url)
        {
            ExtendedWebBrowser browser = this.New(false, url.ToString());
            browser.Navigate(url);
        }

        public void SetTitleName(int tabIndex, string title)
        {
            this._tabControl.InvokeIfRequired(tc => (tc as TabControl).TabPages[tabIndex].Text = title);
        }

        public string GetTitleName(int tabIndex)
        {
            string text = string.Empty;
            _tabControl.InvokeIfRequired(tc => text = (tc as TabControl).TabPages[tabIndex].Text);
            return text;
        }

        public event EventHandler<TextChangedEventArgs> StatusTextChanged;

        public void ChangeActiveBrowser(int index)
        {
            _tabControl.InvokeIfRequired(tc => (tc as TabControl).SelectTab(index));
        }

        public ExtendedWebBrowser ActiveBrowser
        {
            get
            {
                TabPage page = null;
                _tabControl.InvokeIfRequired(tc => page = (tc as TabControl).SelectedTab);
                if (page != null)
                {
                    BrowserControl control = page.Tag as BrowserControl;
                    if (control != null)
                        return control.WebBrowser;
                }
                return null;
            }
        }

        public event EventHandler<CommandStateEventArgs> CommandStateChanged;

        public ExtendedWebBrowser this[int index]
        {
            get
            {
                TabPage page = null;

                _tabControl.InvokeIfRequired(tc => page = (tc as TabControl).TabPages[index]);
                if (page != null)
                {
                    BrowserControl control = page.Tag as BrowserControl;
                    if (control != null)
                        return control.WebBrowser;
                }
                return null;
            }
        }

        public int LastTabIndex
        {
            get
            {
                int lastTabIndex = 0;
                _tabControl.InvokeIfRequired(tc => lastTabIndex = (tc as TabControl).TabCount - 1);
                return lastTabIndex;
            }
        }

        public void CloseAllTabs()
        {
            while (LastTabIndex > -1)
            {
                Close(LastTabIndex);
            }
        }
    }
}
