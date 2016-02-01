using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carubbi.CaptchaBreaker.Interfaces;

namespace Carubbi.CaptchaBreaker.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoteGSACaptchaBreaker : ICaptchaBreaker
    {
        /// <summary>
        /// Método responsavel por realizar a quebra do captcha
        /// </summary>
        /// <param name="image">Byte Array da imagem captcha</param>
        /// <param name="config">Objeto contendo informações relevantes para solucionar o captcha</param>
        /// <returns>String com a solução do captcha</returns>
        public string Break(byte[] image, DTOs.CaptchaConfig config)
        {
            Proxy.DTOs.CaptchaConfig proxyConfig = new Proxy.DTOs.CaptchaConfig();
            proxyConfig.ExecutarCalculo = config.ExecutarCalculo;
            proxyConfig.CaseSensitive = config.CaseSensitive;
            proxyConfig.NumeroDePalavras = config.NumeroDePalavras;
            proxyConfig.TamanhoMaximo = config.TamanhoMaximo;
            proxyConfig.TamanhoMinimo = config.TamanhoMinimo;
            proxyConfig.Tipo = ParseEnum(config.Tipo);

            Proxy.GSACaptchaBreakerClient cl = new Proxy.GSACaptchaBreakerClient();
            return cl.Break(image, proxyConfig);
        }

        private Proxy.DTOs.TipoCaptcha ParseEnum(DTOs.TipoCaptcha tipoCaptcha)
        {
            switch (tipoCaptcha)
            {
                case Carubbi.CaptchaBreaker.DTOs.TipoCaptcha.ApenasLetras:
                    return Proxy.DTOs.TipoCaptcha.ApenasLetras;
                case Carubbi.CaptchaBreaker.DTOs.TipoCaptcha.ApenasNumeros:
                    return Proxy.DTOs.TipoCaptcha.ApenasNumeros;
                case Carubbi.CaptchaBreaker.DTOs.TipoCaptcha.Alfanumerico:
                    return Proxy.DTOs.TipoCaptcha.Alfanumerico;
                default:
                    return Proxy.DTOs.TipoCaptcha.Alfanumerico;
            }
        }
    }
}
