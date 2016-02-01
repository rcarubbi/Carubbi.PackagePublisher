using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Carubbi.AuthenticationManager
{
    /// <summary>
    /// Gerenciador de Autenticação baseado na abordagem do Asp.net Form based Authentication
    /// </summary>
    public class FormBasedAuthenticationManager : IAuthenticationManager
    {
        /// <summary>
        /// Monta um formulário HTML padrão de autenticação e inclui o mesmo no Response da requisição
        /// </summary>
        public void ExibirTelaLogin()
        { 
            StringBuilder stbHtml = new StringBuilder();
            stbHtml.AppendLine("<div  class=\"login-body\">");
                stbHtml.AppendLine("<div class=\"login-field\"><label for=\"username\">Usuário</label>");
                stbHtml.AppendLine("<input type=\"text\" id=\"username\"/></div>");
                stbHtml.AppendLine("<div class=\"login-field\"><label for=\"password\">Senha</label>");
                stbHtml.AppendLine("<input type=\"password\"  id=\"password\" /></div>");
            stbHtml.AppendLine("</div>");
            stbHtml.AppendLine("<div class=\"footer\">");
                stbHtml.AppendLine("<input type=\"checkbox\"  id=\"rememberMe\"   /><label for=\"rememberMe\">Lembrar-me</label>");
                stbHtml.AppendLine("<div>");
                    stbHtml.AppendLine("<button type=\"submit\" id=\"btn-login\" class=\"btn btn-success\" style=\"vertical-align: middle;\">Login</button>");
                stbHtml.AppendLine("</div>");
            stbHtml.AppendLine("</div>");
            stbHtml.AppendLine("<div id=\"error-message\"></div>");
            HttpContext.Current.Response.Write(stbHtml.ToString());
        }


        /// <summary>
        /// Indica se o contexto HTTP está autenticado
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        /// <returns></returns>
        public bool IsAuthenticated(HttpContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Recupera dados customizados do usuário autenticado no contexto HTTP informado
        /// </summary>
        /// <param name="context">contexto HTTP</param>
        /// <returns>dados customizados do usuário</returns>
        public string GetUserData(HttpContext context) 
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var id = (FormsIdentity)context.User.Identity;
                var ticket = id.Ticket;
                return ticket.UserData;
            }
            return string.Empty;
        }

        /// <summary>
        /// Recupera o nome do usuário autenticado no contexto HTTP informado
        /// </summary>
        /// <param name="context">contexto HTTP</param>
        /// <returns>Nome do usuário</returns>
        public String GetUserName(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var id = (FormsIdentity)context.User.Identity;
                var ticket = id.Ticket;
                return ticket.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// Executa o processo de autenticação baseando-se na regra de autenticação informada como parâmetro
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        /// <param name="username">Login do usuário</param>
        /// <param name="rememberMe">indica se deve ser armazenado um cookie permanente na máquina do cliente</param>
        /// <param name="userData">dados customizados do usuário</param>
        /// <param name="authenticationHandler">Método que contem a regra de autenticação</param>
        /// <returns></returns>
        public AuthenticationResult Logon(HttpContext context, string username, bool rememberMe, string userData, Func<bool> authenticationHandler)
        {
            AuthenticationResult result = new AuthenticationResult();
            FormsAuthenticationTicket tkt;
            string cookiestr;
            HttpCookie ck;

            if (authenticationHandler())
            {
                tkt = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(30), rememberMe, userData);
                cookiestr = FormsAuthentication.Encrypt(tkt);
                ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                
                if (rememberMe)
                {
                    ck.Expires = tkt.Expiration;
                    ck.Path = FormsAuthentication.FormsCookiePath;
                }
                
                context.Response.Cookies.Add(ck);
                string strRedirect;
                strRedirect = context.Request["ReturnUrl"];

                if (strRedirect != null)
                {
                    result.ReturnUrl = strRedirect;
                }
                result.Success = true;

            }
            else
            {
                result.Success = false;
            }

            return result;
        }


        /// <summary>
        /// Efetua o logoff do usuário no contexto HTTP informado
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        public void Logoff(HttpContext context)
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Redireciona o contexto HTTP corrente para a url informada no AuthenticationResult caso haja uma
        /// </summary>
        /// <param name="result"></param>
        public void RedirecionarUrl(AuthenticationResult result)
        {
            if (!string.IsNullOrEmpty(result.ReturnUrl))
            {
                HttpContext.Current.Response.Redirect(result.ReturnUrl);
            }
        }
    }
}
