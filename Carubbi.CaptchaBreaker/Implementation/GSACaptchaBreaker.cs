using System;
using Carubbi.CaptchaBreaker.DTOs;
using Carubbi.CaptchaBreaker.Interfaces;
namespace Carubbi.CaptchaBreaker.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class GSACaptchaBreaker : ICaptchaBreaker
    {
        #region ICaptchaBreaker Members

        private Uri _url;
        
        /// <summary>
        /// Busca a url do servidor de captcha no arquivo de configurações da aplicação na seção CarubbiCaptchaBreaker na chave URL_CAPTCHA_BREAKER_SERVER
        /// </summary>
        public GSACaptchaBreaker()
        {
            _url = new Uri(ConfigManager.GetInstance().URL_CAPTCHA_BREAKER_SERVER);
        }
       
        /// <summary>
        /// Resolve o captcha
        /// </summary>
        /// <param name="image">Imagem do captcha</param>
        /// <param name="config">Objeto de configurações</param>
        /// <returns>Solução do captcha</returns>
        public string Break(byte[] image, CaptchaConfig config)
        {
            var client = new CCProto();
            client.Login(_url.Host, _url.Port, "dummy", "dummy");
            var result = client.picture2(image, 30, (int)PictureType.Asirra);
            return result.text;
        }

        #endregion
    }
}
