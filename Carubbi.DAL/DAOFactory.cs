using System.Data;
using Carubbi.Utils.IoC;
using Carubbi.DAL.Interfaces;

namespace Carubbi.DAL
{
    /// <summary>
    /// Padrão Fábrica responsável pela criação do Objeto DAO - Implementação Padrão
    /// </summary>
    public class DAOFactory : IDAOFactory
    {
        private DAOFactory()
        {

        }

        private static volatile DAOFactory _instance;
       
        private static volatile object _locker = new object();


        /// <summary>
        /// Criação do comando a partir do arquivo de configurações de implementação;
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            return ImplementationResolver.Resolve<IDbCommand>();
        }


        /// <summary>
        /// Criação da classe de conexão a partir do arquivo de configurações de implementação
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return ImplementationResolver.Resolve<IDbConnection>();
        }

        /// <summary>
        /// Recupera a instância singleton da Fábrica
        /// </summary>
        /// <returns></returns>
        public static IDAOFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new DAOFactory();
                    }
                }
            }
            return _instance;

        }
    }
}
