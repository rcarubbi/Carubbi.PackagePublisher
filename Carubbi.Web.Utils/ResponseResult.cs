
namespace Carubbi.Web.Utils
{
    /// <summary>
    /// Enumerador que representa as possíveis respostas obtidas em um documento HTML
    /// </summary>
    public enum ResponseType : int
    {
        /// <summary>
        /// Retornado quando é Disparado um alert no documento
        /// </summary>
        Alert,
        /// <summary>
        /// Disparado quando ocorre um postback na página
        /// </summary>
        Postback,
        /// <summary>
        /// Disparado quando ocorre um Window.open() no javascript
        /// </summary>
        WindowOpen,
        /// <summary>
        /// Disparado quando o tempo limite para espera de um evento é atingido
        /// </summary>
        Timeout,
        /// <summary>
        /// Disparado quando uma ação desconhecida não listada acima ocorre
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Estrutura de dados para representar uma resposta encontrada em uma página html
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// Tipo da resposta
        /// </summary>
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// Texto adicional ao tipo da resposta (é preenchido contextualmente)
        /// <example>
        /// No caso do ResponteType ser Alert o texto é o texto contido na janela Alert
        /// No caso de Window.open, o texto é a Url da nova janela
        /// </example>
        /// </summary>
        public string ResponseText { get; set; }
    }
}
