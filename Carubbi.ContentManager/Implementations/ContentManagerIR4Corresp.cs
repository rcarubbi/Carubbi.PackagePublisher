using Carubbi.ContentManager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Carubbi.Extensions;

namespace Carubbi.ContentManager.Implementations
{

    /// <summary>
    /// Implementação do Gerenciador de Conteúdo como facade do proxy do serviço do content manager configurado para uso na sigla IR4 do itaú
    /// </summary>
    public class ContentManagerIR4Corresp : IContentManager
    {
        #region IContentManager Members


        private string _url;
        private string _username;
        private string _password;
        private string _serverName;

        public ContentManagerIR4Corresp(string url,
                               string username,
                               string password,
                                string serverName)
        {
            _url = url;
            _username = username;
            _password = password;
            _serverName = serverName;
        }


        /// <summary>
        /// Envia um arquivo para o servidor a partir do caminho local do mesmo
        /// </summary>
        /// <param name="fullPath">Caminho local do arquivo</param>
        /// <returns>Id do arquivo gerado pelo content manager</returns>
        public string UploadFile(string fullPath)
        {
            FileInfo sourceFileInfo = new FileInfo(fullPath);

            // Cria uma nova instancia do WebServices 
            IR4CorrespProxy content = new IR4CorrespProxy();

            content.RequireMtom = true;

            // Aponta a instancia atual do WebService para a URL de desenvolvimento: 
            content.Url = _url; //"http://cmdes.itau/CMBSpecificWebService/services/CMWebService";

            // Aponta a instancia atual do WebService para a URL de produção: 
            //content.Url = http://cmcorp2.itau/CMBSpecificWebService/services/CMWebService"; 
            // Cria uma requisição de Criação de Item "CreateItemRequest" 
            CreateItemRequest Request = new CreateItemRequest();
            // Autenticação 
            Request.AuthenticationData = new AuthenticationData();
            Request.AuthenticationData.ServerDef = new ServerDef();
            //Request.AuthenticationData.ServerDef.ServerName = "NLSDB01"; // PRODUCAO 
            Request.AuthenticationData.ServerDef.ServerName = _serverName;// "NLSDBDES";  // DESENVOLVIMENTO 
            Request.AuthenticationData.ServerDef.ServerType = ServerType.ICM;
            Request.AuthenticationData.LoginData = new AuthenticationDataLoginData();
            Request.AuthenticationData.LoginData.UserID = _username;// "USUARIO";
            Request.AuthenticationData.LoginData.Password = _password;// "SENHA";
            // Cria um novo Item de requisição 
            // Isto é necessário pois esta propriedade não é automaticamente criada pelo 
            //"CreateItemRequest" 
            Request.Item = new CreateItemRequestItem();




            // Cria um novo Item de XML 
            Request.Item.ItemXML = new ItemXML();

            // Associa o item ao OA0_ExemploCM 
            Request.Item.ItemXML.IR4_CORRESP = new IR4_CORRESP();
            // Atribui as propriedades do arquivo nos campos do OA0_ExemploCM. 
            //Request.Item.ItemXML.IR4_CORRESP.IR4_COD_CORRESP = "Titulo de Teste"; 

            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT = new ICMBASETEXT[1];
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0] = new ICMBASETEXT();
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject = new LobObjectType();
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.label = new LobObjectTypeLabel();
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.label.name = sourceFileInfo.FullName;
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.originalFileName = sourceFileInfo.Name;
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.MIMEType = sourceFileInfo.Extension.GetMimeType();

            // Definições do arquivo que será incluído 
            MTOMAttachment[] attachments = new MTOMAttachment[1];
            attachments[0] = new MTOMAttachment();
            attachments[0].ID = sourceFileInfo.Name;
            attachments[0].MimeType = sourceFileInfo.Extension.GetMimeType();
            attachments[0].Value = File.ReadAllBytes(sourceFileInfo.FullName);

            Request.mtomRef = new MTOMAttachment[1];
            Request.mtomRef[0] = attachments[0];

            // Realiza a chamada da requisição de novo item, e retorna um "Reply", contendo  
            // informações sobre a situação da requisição. 
            CreateItemReply reply = content.CreateItem(Request);

            if (reply.RequestStatus.success)
            {
                //Console.WriteLine("Arquivo criado com sucesso: " + reply.Item.URI); 
                Uri uri = new Uri(reply.Item.URI);

                HttpRequest req = new HttpRequest(null, uri.ToString(), uri.Query.Substring(1)); // uri.Query inclui o '?', o que o HttpRequest não espera

                return req.Params["pid"];
            }
            else
            {
                //Console.WriteLine("Falha ao criar arquivo"); 
                ErrorData[] err = reply.RequestStatus.ErrorData;
                string errorStack = "";
                for (int i = 0; i < err.Length; i++)
                {
                    errorStack += "Item: " + err[i].Item + " Code: " + err[i].returnCode + "Stack: " + err[i].stackTrace + "\n";
                }
                throw new Exception(errorStack);
            }
        }

        /// <summary>
        /// Retorna a lista de arquivos no servidor associada a um Id
        /// </summary>
        /// <param name="idContent">Chave de busca</param>
        /// <returns>Lista de arquivos</returns>
        public List<byte[]> GetFiles(string idContent)
        {
            List<byte[]> files = new List<byte[]>();

            // Cria uma nova instancia do WebServices 
            IR4CorrespProxy content = new IR4CorrespProxy();

            content.RequireMtom = true;

            // Aponta a instancia atual do WebService para a URL de desenvolvimento: 
            content.Url = _url; //"http://cmdes.itau/CMBSpecificWebService/services/CMWebService";

            // Aponta a instancia atual do WebService para a URL de produção: 
            //content.Url = http://cmcorp2.itau/CMBSpecificWebService/services/CMWebService"; 
            // Cria uma requisição de consulta de arquivos 
            RetrieveItemRequest Request = new RetrieveItemRequest();
            // Autenticação 
            Request.AuthenticationData = new AuthenticationData();
            Request.AuthenticationData.ServerDef = new ServerDef();
            //Request.AuthenticationData.ServerDef.ServerName = "NLSDB01"; // PRODUCAO 
            Request.AuthenticationData.ServerDef.ServerName = _serverName;// "NLSDBDES";  // DESENVOLVIMENTO 
            Request.AuthenticationData.ServerDef.ServerType = ServerType.ICM;
            Request.AuthenticationData.LoginData = new AuthenticationDataLoginData();
            Request.AuthenticationData.LoginData.UserID = _username;// "USUARIO";
            Request.AuthenticationData.LoginData.Password = _password; // "SENHA";

            Request.retrieveOption = RetrieveItemRequestRetrieveOption.CONTENT;
            Request.contentOption = RetrieveItemRequestContentOption.ATTACHMENTS;
            Request.Item = new RetrieveItemRequestItem[1];
            Request.Item[0] = new RetrieveItemRequestItem();
            Request.Item[0].URI = idContent; // ID DO DOCUMENTO 

            RetrieveItemReply reply = content.RetrieveItem(Request);
            if (reply.RequestStatus.success)
            {
                MTOMAttachment[] attachments = reply.mtomRef;

                for (int j = 0; j < attachments.Length; j++)
                {
                    string id = attachments[j].ID;
                    files.Add(attachments[j].Value);
                }
            }
            else
            {
                //Console.WriteLine("Falha ao criar arquivo"); 
                ErrorData[] err = reply.RequestStatus.ErrorData;
                string errorStack = "";
                for (int i = 0; i < err.Length; i++)
                {
                    errorStack += "Item: " + err[i].Item + " Code: " + err[i].returnCode + "Stack: " + err[i].stackTrace + "\n";
                }
                throw new Exception(errorStack);
            }
            return files;
        }
      
        /// <summary>
        /// Envia um arquivo para o servidor a partir do conteudo e nome original, dando a opção de gravar no servidor com um novo nome
        /// </summary>
        /// <param name="binaryData">Dados do arquivo</param>
        /// <param name="originalFilename">Nome de origem</param>
        /// <param name="filename">Nome de destino</param>
        /// <returns>Id do arquivo gerado pelo content manager</returns>
        public string UploadFile(Stream binaryData, string originalFilename, string filename)
        {
            // Cria uma nova instancia do WebServices 
            IR4CorrespProxy content = new IR4CorrespProxy();

            content.RequireMtom = true;

            // Aponta a instancia atual do WebService para a URL de desenvolvimento: 
            content.Url = _url; //"http://cmdes.itau/CMBSpecificWebService/services/CMWebService";

            // Aponta a instancia atual do WebService para a URL de produção: 
            //content.Url = http://cmcorp2.itau/CMBSpecificWebService/services/CMWebService"; 
            // Cria uma requisição de Criação de Item "CreateItemRequest" 
            CreateItemRequest Request = new CreateItemRequest();
            // Autenticação 
            Request.AuthenticationData = new AuthenticationData();
            Request.AuthenticationData.ServerDef = new ServerDef();
            //Request.AuthenticationData.ServerDef.ServerName = "NLSDB01"; // PRODUCAO 
            Request.AuthenticationData.ServerDef.ServerName = _serverName;// "NLSDBDES";  // DESENVOLVIMENTO 
            Request.AuthenticationData.ServerDef.ServerType = ServerType.ICM;
            Request.AuthenticationData.LoginData = new AuthenticationDataLoginData();
            Request.AuthenticationData.LoginData.UserID = _username;// "USUARIO";
            Request.AuthenticationData.LoginData.Password = _password;// "SENHA";
            // Cria um novo Item de requisição 
            // Isto é necessário pois esta propriedade não é automaticamente criada pelo 
            //"CreateItemRequest" 
            Request.Item = new CreateItemRequestItem();

            // Cria um novo Item de XML 
            Request.Item.ItemXML = new ItemXML();

            // Associa o item ao OA0_ExemploCM 
            Request.Item.ItemXML.IR4_CORRESP = new IR4_CORRESP();
            // Atribui as propriedades do arquivo nos campos do OA0_ExemploCM. 
            //Request.Item.ItemXML.IR4_CORRESP.IR4_COD_CORRESP = "Titulo de Teste"; 

            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT = new ICMBASETEXT[1];
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0] = new ICMBASETEXT();
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject = new LobObjectType();
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.label = new LobObjectTypeLabel();
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.label.name = filename;
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.originalFileName = originalFilename;
            Request.Item.ItemXML.IR4_CORRESP.ICMBASETEXT[0].resourceObject.MIMEType = Path.GetExtension(originalFilename).GetMimeType();


            // Definições do arquivo que será incluído 
            MTOMAttachment[] attachments = new MTOMAttachment[1];
            attachments[0] = new MTOMAttachment();
            attachments[0].ID = originalFilename;
            attachments[0].MimeType = Path.GetExtension(originalFilename).GetMimeType();
            attachments[0].Value = binaryData.ToByteArray();

            Request.mtomRef = new MTOMAttachment[1];
            Request.mtomRef[0] = attachments[0];

            // Realiza a chamada da requisição de novo item, e retorna um "Reply", contendo  
            // informações sobre a situação da requisição. 
            CreateItemReply reply = content.CreateItem(Request);

            if (reply.RequestStatus.success)
            {
                //Console.WriteLine("Arquivo criado com sucesso: " + reply.Item.URI); 
                Uri uri = new Uri(reply.Item.URI);

                HttpRequest req = new HttpRequest(null, uri.ToString(), uri.Query.Substring(1)); // uri.Query inclui o '?', o que o HttpRequest não espera

                return req.Params["pid"];
            }
            else
            {
                //Console.WriteLine("Falha ao criar arquivo"); 
                ErrorData[] err = reply.RequestStatus.ErrorData;
                string errorStack = "";
                for (int i = 0; i < err.Length; i++)
                {
                    errorStack += "Item: " + err[i].Item + " Code: " + err[i].returnCode + "Stack: " + err[i].stackTrace + "\n";
                }
                throw new Exception(errorStack);
            }
        }

        #endregion
    }
}
