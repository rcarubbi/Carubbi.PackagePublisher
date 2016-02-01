using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Carubbi.AuthenticationManager
{
    /// <summary>
    /// Interface do gerenciador de autenticações
    /// </summary>
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Monta um formulário HTML padrão de autenticação e inclui o mesmo no Response da requisição
        /// </summary>
        void ExibirTelaLogin();


        /// <summary>
        /// Indica se o contexto HTTP está autenticado
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        /// <returns></returns>
        bool IsAuthenticated(HttpContext context);

        /// <summary>
        /// Recupera dados customizados do usuário autenticado no contexto HTTP informado
        /// </summary>
        /// <param name="context">contexto HTTP</param>
        /// <returns>dados customizados do usuário</returns>
        string GetUserData(HttpContext context);

        /// <summary>
        /// Recupera o nome do usuário autenticado no contexto HTTP informado
        /// </summary>
        /// <param name="context">contexto HTTP</param>
        /// <returns>Nome do usuário</returns>
        String GetUserName(HttpContext context);

        /// <summary>
        /// Executa o processo de autenticação baseando-se na regra de autenticação informada como parâmetro
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        /// <param name="username">Login do usuário</param>
        /// <param name="rememberMe">indica se deve ser armazenado um cookie permanente na máquina do cliente</param>
        /// <param name="userData">dados customizados do usuário</param>
        /// <param name="authenticationHandler">Método que contem a regra de autenticação</param>
        /// <returns></returns>
        AuthenticationResult Logon(HttpContext context, string username, bool rememberMe, string userData, Func<bool> authenticationHandler);

        /// <summary>
        /// Efetua o logoff do usuário no contexto HTTP informado
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        void Logoff(HttpContext context);

        /// <summary>
        /// Redireciona o contexto HTTP corrente para a url informada no AuthenticationResult caso haja uma
        /// </summary>
        /// <param name="result"></param>
        void RedirecionarUrl(AuthenticationResult result);
    }
}
