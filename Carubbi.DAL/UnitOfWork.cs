using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using Carubbi.Utils.IoC;
using Carubbi.DAL.Interfaces;

namespace Carubbi.DAL
{
    /// <summary>
    /// Classe que implementa o Padrão UnitOfWork - Gerenciador de Transações com o banco de dados
    /// <para>Encapsula os objetos Connection e Transaction do .net</para>
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        /// <summary>
        /// Controi o objeto com uma conexão com o banco de dados aberta
        /// </summary>
        public UnitOfWork()
        {
            Connection = GetConnection();
            OpenConnection();
        }

        /// <summary>
        /// Recupera uma conexão nova ou existente
        /// <para>
        /// Quando nova, a criação da conexão é delegada à factory responsável pela criação da mesma.
        /// A Factory é definida a partir do arquivo de configurações que resolve as implementações a partir das interfaces (Implementations.Config)
        /// <seealso cref="ImplementationResolver"/>  
        /// Na ausencia de um arquivo de configurações de implementação, a factory padrão é assumida.
        /// </para>
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetConnection()
        {
            IDbConnection connection = null;
            if (Connection == null)
            {
                IDAOFactory factory = ImplementationResolver.ResolveSingleton<IDAOFactory>();
                connection = (factory ?? DAOFactory.GetInstance()).CreateConnection();
            }
            else
            {
                connection = Connection;
            }
            return connection;
        }

        /// <summary>
        /// Abre a conexão a partir da connection string encontrada no arquivo de configurações da aplicação (seção ConnectionStrings)
        /// A chave da connectionString deve estar configurada na classe DataBaseConventions <seealso cref="DataBaseConventions"/>
        /// <para>
        /// Após a abertura da conexão uma nova transação é iniciada
        /// A conexão possui escopo no nível da classe, portanto se este método for chamado mais de uma vez, a conexão já estará aberta e o método não surtirá efeito.
        /// </para>
        /// </summary>
        protected void OpenConnection()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.ConnectionString = ConfigurationManager.ConnectionStrings[DataBaseConventions.ConnectionStringKey].ConnectionString;
                Connection.Open();
            }
            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        /// <summary>
        /// Getter e Setter do objeto de Transação 
        /// </summary>
        internal IDbTransaction Transaction
        {
            get;
            set;
        }
 
        /// <summary>
        /// Fecha e descarta a conexão corrente
        /// </summary>
        protected void ReleaseConnection()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
             
            }
        }

        private IDbConnection _connection;

        /// <summary>
        /// Getter e Setter do objeto de conexão
        /// </summary>
        internal IDbConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                if (_connection != null)
                {
                    ReleaseConnection();
                }
                _connection = value;
            }
        }
 
        #region IDisposable Members

        /// <summary>
        /// Commita a transação corrente caso exista uma, e em seguida fecha e libera a conexão.
        /// </summary>
        public void Dispose()
        {
            Commit();
            ReleaseConnection();
        }

        #endregion


        /// <summary>
        /// Cancela as operações pendentes na transação corrente
        /// </summary>
        internal void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
            ReleaseConnection();
        }


        /// <summary>
        /// Commita as operações pendentes
        /// </summary>
        internal void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }
    }
}
