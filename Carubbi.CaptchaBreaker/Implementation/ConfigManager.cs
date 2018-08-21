using Carubbi.ServiceLocator;

namespace Carubbi.CaptchaBreaker.Implementation
{
    /// <summary>
    /// Acessa chaves de configurações em seções customizadas
    /// </summary>
    internal class ConfigManager
    {
        private static AppSettings configSection;
        
        private ConfigManager()
        {
            configSection = new AppSettings("CarubbiCaptchaBreaker");
            CAMINHO_ARQUIVO_IDIOMA = configSection["CAMINHO_ARQUIVO_IDIOMA"];
            URL_CAPTCHA_BREAKER_SERVER = configSection["URL_CAPTCHA_BREAKER_SERVER"];
        }

        internal string CAMINHO_ARQUIVO_IDIOMA { get; private set; }
        
        internal string URL_CAPTCHA_BREAKER_SERVER { get; private set; }
      
        private static volatile ConfigManager _instance;
        
        private static volatile object _locker = new object();
        
        internal static ConfigManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigManager();
                    }
                }
            }
            return _instance;
        }
    }
}
