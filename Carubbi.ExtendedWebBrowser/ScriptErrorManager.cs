using System;
using System.Windows.Forms;

namespace Carubbi.ExtendedWebBrowser
{
    class ScriptErrorManager 
    {
        private ScriptErrorManager()
        {
            _scriptErrors = new NotifyCollection<ScriptError>();
        }

        private NotifyCollection<ScriptError> _scriptErrors;

        private static object lockObject = new object();
        
        private static ScriptErrorManager _instance;

        public static ScriptErrorManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new ScriptErrorManager();

                            /*var scriptErrorManagerType = typeof(ScriptErrorManager<>);
                            Type genericType = typeof(TErrorWindow);
                            Type constructedType = scriptErrorManagerType.MakeGenericType(genericType);
                            _instance = (ScriptErrorManager<TErrorWindow>)Activator.CreateInstance(constructedType);*/
                        }
                    }
                }
                return _instance;
            }
        }

        public NotifyCollection<ScriptError> ScriptErrors
        {
            get
            {
                return _scriptErrors;
            }
        }

        public Form ErrorForm { get; set; }

        public bool ShowErrors { get; set; }

        public void RegisterScriptError(Uri url, string description, int lineNumber)
        {
            this._scriptErrors.Add(new ScriptError(url, description, lineNumber));
            if (ShowErrors)
            {
                ShowWindow();
            }
        }

        public void ShowWindow()
        {
            if (ErrorForm == null || ErrorForm.IsDisposed)
            {
                ErrorForm = Activator.CreateInstance(ErrorForm.GetType()) as Form;
            }
            ErrorForm.Show();
        }

    }
}
