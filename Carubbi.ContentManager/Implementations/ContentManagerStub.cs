using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Carubbi.ContentManager.Interfaces;
using Carubbi.Utils.Persistence;
using System.Reflection;
namespace Carubbi.ContentManager.Implementations
{
    public class ContentManagerStub : IContentManager
    {
        Dictionary<string, string> _indice = new Dictionary<string,string>();

        private string _folderPath = @"C:\ContentManagerMock";
        private string _url; 
        private string _username; 
        private string _password;
        private string _serverName;


        public ContentManagerStub(string url,
                               string username,
                               string password,
                                string serverName)
        {
            _url = url;
            _username = username;
            _password = password;
            _serverName = serverName;
            var indexPath = Path.Combine(_folderPath, "indice.txt");

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

    
            if (!File.Exists(indexPath))
            {
                File.Create(indexPath);
            }
        }

        #region IContentManager Members

       



        private Dictionary<string, string> Indice
        {
            get { return _indice == null ? CarregaIndice() : _indice; }
            set { _indice = value; }
        }

        private Dictionary<string, string> CarregaIndice()
        {
            int counter = 0;
            string line;
            _indice = new Dictionary<string,string>();
            System.IO.StreamReader file = new System.IO.StreamReader(Path.Combine(_folderPath, "indice.txt"));
            while ((line = file.ReadLine()) != null)
            {
                string[] item = line.Split('|');
                _indice.Add(item[0], item[1]);
                counter++;
            }

            file.Close();

            return _indice;
        }

        private string FindFileName(string idContent) {
            AtualizaIndice();
            string arquivo;
            Indice.TryGetValue(idContent, out arquivo);
            return arquivo;
        }

        private string NextContentId() {
            AtualizaIndice();
            long contentId = 1;
            if (Indice.Count > 0) {
                contentId = Int64.Parse(Indice.Keys.Last()) + 1;
            }
            return contentId.ToString("0000000000000000");
        }

        private string NewFileName() {
            return Guid.NewGuid().ToString();
        }

        private void AtualizaIndice() {
            CarregaIndice();
        }

        private void IncrementaIndice(string contentId, string fileName) {
            using (StreamWriter w = File.AppendText(Path.Combine(_folderPath, "indice.txt")))
            {
                w.Write(String.Format("{0}|{1}{2}", contentId, fileName, Environment.NewLine));
            }
            AtualizaIndice();
        }

        /// <summary>
        /// Envia um arquivo para o servidor a partir do caminho local do mesmo
        /// </summary>
        /// <param name="fullPath">Caminho local do arquivo</param>
        /// <returns>Id do arquivo gerado pelo índice</returns>
        public string UploadFile(string fullPath)
        {
            string newFileName = NewFileName();
            string destFile = Path.Combine(_folderPath, newFileName); 
            string contentId = NextContentId();

            File.Move(fullPath, destFile);

            IncrementaIndice(contentId, newFileName);
            return contentId;
        }


        /// <summary>
        /// Retorna a lista de arquivos no servidor associada a um Id
        /// </summary>
        /// <param name="idContent">Chave de busca</param>
        /// <returns>Lista de arquivos</returns>
        public List<byte[]> GetFiles(string idContent)
        {
            List<byte[]> files = new List<byte[]>();
            AtualizaIndice();

            string sourceFile = Path.Combine(_folderPath, FindFileName(idContent));

            FileStream fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            byte[] binaryData = new byte[fileStream.Length];

            fileStream.Read(binaryData, 0, (int)fileStream.Length);
            fileStream.Close();

            files.Add(binaryData);

            return files;
        }

        /// <summary>
        /// Envia um arquivo para o servidor a partir do conteudo e nome original, dando a opção de gravar no servidor com um novo nome
        /// </summary>
        /// <param name="binaryData">Dados do arquivo</param>
        /// <param name="originalFilename">Nome de origem</param>
        /// <param name="filename">Nome de destino</param>
        /// <returns>Id do arquivo gerado pelo índice</returns>
        public string UploadFile(Stream binaryData, string originalFilename, string filename)
        {
            string newFileName = NewFileName();
            string destFile = Path.Combine(_folderPath, newFileName); 
            string contentId = NextContentId();

            FileStream fs = File.Create(destFile, 2048, FileOptions.None);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(binaryData.ToByteArray());

            bw.Close();
            fs.Close();

            IncrementaIndice(contentId, newFileName);
            return contentId;
        }

        #endregion
    }
}
