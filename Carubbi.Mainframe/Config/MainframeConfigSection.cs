using System.Configuration;
namespace Carubbi.Mainframe.Config
{
    /// <summary>
    /// Seção de Configuração "Mainframe" pode conter as configurações de 1 ou mais terminais
    /// </summary>
    public class MainframeConfigSection : ConfigurationSection
    {
        private static MainframeConfigSection _mainframeSection = ConfigurationManager.GetSection("Mainframe") as MainframeConfigSection;

        public static MainframeConfigSection MainframeSection
        {
            get
            {
                return _mainframeSection;
            }
        }

        [ConfigurationProperty("Terminals")]
        [ConfigurationCollection(typeof(MainframeConfigCollection))]
        public MainframeConfigCollection MainframeTerminals
        {
            get { return this["Terminals"] as MainframeConfigCollection; }
        }
    }
}
