using System;
using System.Runtime.Serialization;
namespace Carubbi.CaptchaBreaker.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CaptchaConfig
    {
        /// <summary>
        /// Seta o número de palavras que será devolvido após o captcha a ser solucionado
        /// </summary>
        [DataMember]
        public int NumeroDePalavras { get; set; }

        /// <summary>
        /// Defini se o captcha a ser solucionado possui letras maiusculas
        /// </summary>
        [DataMember]
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Defini qual o tipo de caractes o captcha possui
        /// </summary>
        [DataMember]
        public TipoCaptcha Tipo { get; set; }

        /// <summary>
        /// Executar Calculo
        /// </summary>
        [Obsolete]
        [DataMember]
        public bool ExecutarCalculo { get; set; }

        /// <summary>
        /// <para>GSACaptchaBreaker: 0 para não validar, aceita de 0 a 20</para>
        /// </summary>
        [DataMember]
        public int TamanhoMinimo { get; set; }

        /// <summary>
        /// Setar qual o formato da imagem gerada para solucionar o captcha
        /// </summary>
        [IgnoreDataMember]
        public System.Drawing.Imaging.ImageFormat TipoImagem { get; set; }

        /// <summary>
        /// <para>GSACaptchaBreaker: 0 para não validar, aceita de 0 a 20</para>
        /// </summary>
        [DataMember]
        public int TamanhoMaximo { get; set; }
   }
}
