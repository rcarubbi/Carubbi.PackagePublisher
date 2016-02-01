
using System.Runtime.Serialization;
namespace Carubbi.CaptchaBreaker.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "TipoCaptcha")]
    public enum TipoCaptcha : byte
    {
        /// <summary>
        /// Tipo para captchas compostos apenas de letras
        /// </summary>
        [EnumMember]
        ApenasLetras,
        
        /// <summary>
        /// Tipo para captchas compostos apenas números
        /// </summary>
        [EnumMember]
        ApenasNumeros,
        
        /// <summary>
        /// Tipo para captchas compostos por caracteres alfanuméricos
        /// </summary>
        [EnumMember]
        Alfanumerico
    }
}
