using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Carubbi.Utils.Configuration
{
    public class TripleDESProtectedConfigurationProvider : ProtectedConfigurationProvider
    {

        private readonly TripleDESCryptoServiceProvider _des =
            new TripleDESCryptoServiceProvider();

        private string _pName;

        // Gets the path of the file 
        // containing the key used to 
        // encryption or decryption. 
        public string KeyFilePath { get; private set; }


        // Gets the provider name. 
        public override string Name => _pName;


        // Performs provider initialization. 
        public override void Initialize(string name, NameValueCollection config)
        {
            _pName = name;
            KeyFilePath = config["keyContainerName"];
            ReadKey(KeyFilePath);
        }


        // Performs encryption. 
        public override XmlNode Encrypt(XmlNode node)
        {
            var encryptedData = EncryptString(node.OuterXml);

            var xmlDoc = new XmlDocument {PreserveWhitespace = true};
            xmlDoc.LoadXml("<EncryptedData>" +
                encryptedData + "</EncryptedData>");

            return xmlDoc.DocumentElement ?? throw new InvalidOperationException();
        }

        // Performs decryption. 
        public override XmlNode Decrypt(XmlNode encryptedNode)
        {
            var decryptedData =
                DecryptString(encryptedNode.InnerText);

            var xmlDoc = new XmlDocument {PreserveWhitespace = true};
            xmlDoc.LoadXml(decryptedData);

            return xmlDoc.DocumentElement ?? throw new InvalidOperationException();
        }

        // Encrypts a configuration section and returns  
        // the encrypted XML as a string. 
        private string EncryptString(string encryptValue)
        {
            var valBytes =
                Encoding.Unicode.GetBytes(encryptValue);

            var transform = _des.CreateEncryptor();

            var ms = new MemoryStream();
            var cs = new CryptoStream(ms,
                transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            var returnBytes = ms.ToArray();
            cs.Close();

            return Convert.ToBase64String(returnBytes);
        }


        // Decrypts an encrypted configuration section and  
        // returns the unencrypted XML as a string. 
        private string DecryptString(string encryptedValue)
        {
            var valBytes =
                Convert.FromBase64String(encryptedValue);

            var transform = _des.CreateDecryptor();

            var ms = new MemoryStream();
            var cs = new CryptoStream(ms,
                transform, CryptoStreamMode.Write);

            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();

            var returnBytes = ms.ToArray();
            cs.Close();

            return Encoding.Unicode.GetString(returnBytes);
        }

        // Generates a new TripleDES key and vector and  
        // writes them to the supplied file path. 
        public void CreateKey(string filePath)
        {
            _des.GenerateKey();
            _des.GenerateIV();

            using (var sw = new StreamWriter(filePath, false))
            {
                sw.WriteLine(ByteToHex(_des.Key));
                sw.WriteLine(ByteToHex(_des.IV));
            }
        }


        // Reads in the TripleDES key and vector from  
        // the supplied file path and sets the Key  
        // and IV properties of the  
        // TripleDESCryptoServiceProvider. 
        private void ReadKey(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                var keyValue = sr.ReadLine();
                var ivValue = sr.ReadLine();
                _des.Key = HexToByte(keyValue);
                _des.IV = HexToByte(ivValue);
            }
        }


        // Converts a byte array to a hexadecimal string. 
        private static string ByteToHex(byte[] byteArray)
        {
            return byteArray.Aggregate("", (current, b) => current + b.ToString("X2"));
        }

        // Converts a hexadecimal string to a byte array. 
        private static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            var i = 0;
            for (; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

    }
}
