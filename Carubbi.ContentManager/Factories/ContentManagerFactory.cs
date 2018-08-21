using Carubbi.ContentManager.Interfaces;
using Carubbi.ServiceLocator;

namespace Carubbi.ContentManager.Factories
{
    /// <summary>
    /// Fábrica de criação de classes Gerenciadoras de Conteúdo
    /// </summary>
    public class ContentManagerFactory
    {

        private ContentManagerFactory()
        {

        }

        private static volatile ContentManagerFactory _instance;

        private static volatile object _locker = new object();

        /// <summary>
        /// Recupera a instância da fábrica através do padrão Singleton
        /// </summary>
        /// <returns></returns>
        public static ContentManagerFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new ContentManagerFactory();
                    }
                }
            }
            return _instance;

        }

        /// <summary>
        /// Cria um gerenciador de conteúdo através do container de injeção de dependência <seealso cref="Utils.IoC.ImplementationResolver"/>
        /// </summary>
        /// <param name="url">Caminho do servidor de gerenciamento de conteúdo</param>
        /// <param name="username">Usuário para autentiação no servidor de gerenciamento de conteúdo</param>
        /// <param name="password">Senha para autentiação no servidor de gerenciamento de conteúdo</param>
        /// <param name="serverName">Nome do servidor de gerenciamento de conteúdo</param>
        /// <returns></returns>
        public IContentManager CreateContentManager(string url,
                               string username,
                               string password,
                                string serverName)
        {
            return ImplementationResolver.Resolve<IContentManager>(new object [] { url, username, password, serverName});
        }

    }
}
