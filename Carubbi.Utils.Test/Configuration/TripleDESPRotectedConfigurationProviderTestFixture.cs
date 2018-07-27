using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using Carubbi.Utils.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Configuration.ConfigurationManager;

namespace Carubbi.Utils.Test
{
    [TestClass]
    public class TripleDESPRotectedConfigurationProviderTestFixture
    {
        [TestMethod]
        public void GivenAConfigFile_WhenSectionIsProtected_SectionDataShoudBeRetrievedProperly()
        {
            var config = OpenExeConfiguration(ConfigurationUserLevel.None);
            var configSection = config.GetSection("connectionStrings") as ConnectionStringsSection;
            var keyPath = Path.Combine(Environment.CurrentDirectory, "key.txt");

          
            FileInfo f = new FileInfo(keyPath);
            if (!f.Exists)
            {
                var protectionProvider = new TripleDESProtectedConfigurationProvider();
                protectionProvider.CreateKey(keyPath);
            }

            var secret = Guid.NewGuid();
            configSection.ConnectionStrings["test"].ConnectionString = secret.ToString();
            configSection.SectionInformation.ProtectSection("ClassUnderTest");
            configSection.SectionInformation.ForceSave = true;
            config.Save();

            ConfigurationManager.RefreshSection("connectionStrings");
            var connection = ConfigurationManager.ConnectionStrings["test"];
            Assert.AreEqual(connection.ConnectionString, secret.ToString());
        }
    }
}
