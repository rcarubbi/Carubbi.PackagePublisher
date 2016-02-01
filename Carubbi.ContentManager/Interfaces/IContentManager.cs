using System.Web;
using System.IO;
using System.Collections.Generic;
namespace Carubbi.ContentManager.Interfaces
{
    public interface IContentManager
    {
        /// <summary>
        /// Envia um arquivo para o servidor a partir do caminho local do mesmo
        /// </summary>
        /// <param name="fullPath">Caminho local do arquivo</param>
        /// <returns>Confirmação de envio (O protocolo de comunicação depende da implementação concreta)</returns>
        string UploadFile(string fullPath);

        /// <summary>
        /// Retorna a lista de arquivos no servidor associada a um Id
        /// </summary>
        /// <param name="idContent">Chave de busca</param>
        /// <returns>Lista de arquivos</returns>
        List<byte[]> GetFiles(string idContent);

        /// <summary>
        /// Envia um arquivo para o servidor a partir do conteudo e nome original, dando a opção de gravar no servidor com um novo nome
        /// </summary>
        /// <param name="binaryData">Dados do arquivo</param>
        /// <param name="originalFilename">Nome de origem</param>
        /// <param name="filename">Nome de destino</param>
        /// <returns>Confirmação de envio (O protocolo de comunicação depende da implementação concreta)</returns>
        string UploadFile(Stream binaryData, string originalFilename, string filename);
    }
}
