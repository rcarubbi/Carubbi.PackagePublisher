using System.Data;
using System.Data.SqlClient;
using Carubbi.DAL.Interfaces;
using System.Configuration;

namespace Itau.BSITokenDALFactory
{

    /// <summary>
    /// Fábrica responsável por recuperar SqlCommand e SqlConnection a partir do componente da BSI
    /// Fachada para encapsular o compoente da BSI de gerenciamento de conexões com bases de dados do Itaú
    /// </summary>
    public class DAOFactory : IDAOFactory
    {

        private BSI.Token.GerenciadorToken gerenciador;

        private DAOFactory()
        {
            gerenciador = new BSI.Token.GerenciadorToken();
        }

        private static volatile DAOFactory _instance;

        private static volatile object _locker = new object();

        /// <summary>
        /// Cria um SqlCommand
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// Cria conexão com o banco a partir do componente de conexão da BSI
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return gerenciador.ConexaoSQL(ConfigurationManager.AppSettings["NOME_BANCO"]);
        }

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
