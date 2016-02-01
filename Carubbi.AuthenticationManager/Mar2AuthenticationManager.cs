using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Itau.SC.Mar2.Web.Security;

namespace Carubbi.AuthenticationManager
{
    /// <summary>
    /// Gerenciador de Autenticação baseado no autenticador do Itaú MAR2
    /// </summary>
    public class Mar2AuthenticationManager : IAuthenticationManager
    {
        /// <summary>
        /// Indica se o contexto HTTP está autenticado
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        /// <returns></returns>
        public bool IsAuthenticated(HttpContext context)
        {
            return Mar2Identity.IsAuthenticated;
        }

        /// <summary>
        /// Recupera dados customizados do usuário autenticado no contexto HTTP informado
        /// </summary>
        /// <param name="context">contexto HTTP</param>
        /// <returns>dados customizados do usuário</returns>
        public string GetUserData(HttpContext context)
        {
            return Mar2Identity.Funcional;
        }

        /// <summary>
        /// Recupera o nome do usuário autenticado no contexto HTTP informado
        /// </summary>
        /// <param name="context">contexto HTTP</param>
        /// <returns>Nome do usuário</returns>
        public String GetUserName(HttpContext context)
        {
            return Mar2Identity.NomeCompleto;
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
            throw new NotSupportedException("Não disponivel na autenticação mar2");
        }

        /// <summary>
        /// Efetua o logoff do usuário no contexto HTTP informado
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        public void Logoff(HttpContext context)
        {
            context.Response.Expires = -1000;
            context.Session.RemoveAll();
        }

        /// <summary>
        /// Monta o formulário padrão de autenticação do MAR2
        /// </summary>
        public void ExibirTelaLogin()
        {
            Mar2Identity.ShowTelaLogin();
        }

        /// <summary>
        /// Redireciona o contexto HTTP corrente para a url informada no AuthenticationResult caso haja uma
        /// </summary>
        /// <param name="result"></param>
        public void RedirecionarUrl(AuthenticationResult result)
        {
            Itau.SC.Mar2.Web.Security.Mar2Pages.RedirectProcessRequest();
        }
    }
}
