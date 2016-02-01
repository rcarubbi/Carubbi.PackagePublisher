using System;
using System.Globalization;
using System.Net;
using System.Text;

namespace Itau.ProxyAuthentication
{
    /// <summary>
    /// Classe para autenticação no proxy do itau para acesso a internet
    /// </summary>
    public static class AuthenticationHelper
    {
        /// <summary>
        /// Autentica a partir da funcional e senha de um usuário do itaú
        /// </summary>
        /// <param name="funcional">Número da Funcional</param>
        /// <param name="senha">Senha de rede do funcionário</param>
        /// <returns>Indicador de sucesso</returns>
        public static bool Authenticate(string funcional, string senha)
        {

            try
            {
                var cleanRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri("http://www.google.com"));
                cleanRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; itx; .NET CLR 1.1.4322)";
                cleanRequest.KeepAlive = true;
                cleanRequest.GetResponse();
                cleanRequest.Abort(); 
                cleanRequest = null;
                return true;
            }
            catch
            {
                try
                {
                    byte[] urlBytes = Encoding.UTF8.GetBytes("http://www.google.com".ToCharArray());
                    var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(string.Format(CultureInfo.CurrentCulture, "https://proxylogin.itau/?cfru={0}", Convert.ToBase64String(urlBytes))));
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; itx; .NET CLR 1.1.4322)";

                    byte[] authBytes = Encoding.UTF8.GetBytes(String.Format(CultureInfo.CurrentCulture, "{0}:{1}", funcional, senha).ToCharArray());
                    request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(authBytes);
                    request.KeepAlive = true;
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    request.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
                    request.Headers["Cookie"] = "BCSI-CS-578f1ddf35ea416c=2";
                    request.GetResponse();
                    request.Abort(); 
                    request = null;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }
    }
}
