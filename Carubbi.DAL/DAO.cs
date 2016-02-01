using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using Carubbi.Utils.Persistence;
using System.Data.SqlTypes;
using Carubbi.DAL.Interfaces;
using Carubbi.Utils.IoC;

namespace Carubbi.DAL
{
    /// <summary>
    /// Classe que encapsula a persistencia de dados de uma determinada entidade T a partir de stored procedures
    /// </summary>
    /// <typeparam name="T">Entidade a ser controlada</typeparam>
    public class DAO<T> : IDAO<T>
    {
        #region Construtores

        /// <summary>
        /// Constrói a DAO utilizando o método de conversão do conversor padrão (por reflexão) de linha da tabela para entidade 
        /// </summary>
        public DAO()
            : this(FieldReflector.Reflect<T>, null)
        {
            _escopoLocal = true;
        }

        private bool _escopoLocal = false;

        /// <summary>
        /// Constrói a DAO utilizando o conversor padrão (por reflexão) de linha da tabela para entidade e Gerenciador de transações (UnitOfWork) <seealso cref="UnitOfWork"/>
        /// </summary>
        /// <param name="unitOfWork">Gerenciador de transações</param>
        public DAO(UnitOfWork unitOfWork)
            : this(FieldReflector.Reflect<T>, unitOfWork)
        {

        }

        /// <summary>
        /// Getter e Setter do objeto UnitOfWork, quando não definido o escopo da transação corrente acaba logo após a execução do comando efetuado por esta classe
        /// </summary>
        public UnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
            set
            {
                _escopoLocal = value == null;
                _unitOfWork = value;
            }
            
        }

        UnitOfWork _unitOfWork;

        /// <summary>
        /// Constrói uma DAO informando um método de conversão da linha da tabela para entidade customizado e um gerenciador de transações UnitOfWork
        /// <seealso cref="UnitOfWork"/>
        /// </summary>
        /// <param name="convertRowHandler">Método de conversão da linha da tabela para a entidade</param>
        /// <param name="unitOfWork">Gerenciador de transações</param>
        public DAO(Func<IDataReader, T> convertRowHandler, UnitOfWork unitOfWork)
        {
            _convertRowHandler = convertRowHandler;
            entityNameConvention = DataBaseConventions.EntityNameConventionHandler != null ? DataBaseConventions.EntityNameConventionHandler : t => t;
            _unitOfWork = unitOfWork;
        }
        #endregion

        /// <summary>
        /// Getter e Setter do Objeto Command
        /// </summary>
        protected IDbCommand Command
        {
            get;
            set;
        }

        /// <summary>
        /// Getter e Setter do Objeto DataReader
        /// </summary>
        protected IDataReader DataReader
        {
            get;
            set;
        }

        /// <summary>
        /// Método responsável por definir convenções nas regras de nome das entidades;
        /// </summary>
        protected EntityNameConventionHandler entityNameConvention;

        /// <summary>
        /// Conversor de linha de tabela em entidade
        /// </summary>
        protected Func<IDataReader, T> _convertRowHandler;

        /// <summary>
        /// Recupera os campos chave da entidade a partir dos parametros marcados como chave com o atributo CampoChaveAttribute
        /// <para>
        /// Caso nenhuma propriedade tenha sido marcada com o atributo e a classe possuir um atributo chamado Id, o mesmo é definido como chave
        /// </para>
        /// </summary>
        /// <param name="entity">Entidade a ser pesquisada</param>
        /// <returns>Dicionario de Campos chave contendo o nome do campo e o valor da instância</returns>
        protected Dictionary<string, Object> ObterCamposChave(T entity)
        {
            Type entityType = typeof(T);

            var propriedadesChave = entityType.GetProperties().Where(p => p.GetCustomAttributes(typeof(CampoChaveAttribute), true).Count() > 0);
            var props = new Dictionary<string, Object>();
            if (propriedadesChave.Count() > 0)
            {
                foreach (var item in propriedadesChave)
                {
                    props.Add(item.Name, item.GetValue(entity, null));
                }
            }
            else if (entityType.GetProperties().Where(p => p.Name == "Id").Count() > 0)
            {
                props.Add("Id", entityType.GetProperties().Single(p => p.Name == "Id").GetValue(entity, null));
            }

            return props;
        }

        /// <summary>
        /// Executa uma procedure sem parâmetros que retorna uma coleção de uma determinada entidade
        /// </summary>
        /// <param name="procedureName">Nome da procedure</param>
        /// <returns></returns>
        protected IEnumerable<T> ExecuteReader(string procedureName)
        {
            return ExecuteReader(procedureName, dr => _convertRowHandler(dr), new { });
        }

        /// <summary>
        /// Executa uma procedure com parametros que retorna uma coleção de uma determinada entidade
        /// </summary>
        /// <param name="procedureName">Nome da procedure</param>
        /// <param name="parameters">Instancia de uma classe que terá suas propriedades refletidas como parâmetros da procedure (exceto propriedades marcadas com CampoNaoPersistivelAttribute)
        /// </param>
        /// <returns>Coleção de objetos da entidade corrente</returns>
        protected IEnumerable<T> ExecuteReader(string procedureName, object parameters)
        {
            return ExecuteReader(procedureName, dr => _convertRowHandler(dr), parameters);
        }

        /// <summary>
        /// Executa uma procedure sem parâmetros definindo um método de conversão de linha da tabela em entidade customizado
        /// </summary>
        /// <param name="procedureName">Nome da procedure</param>
        /// <param name="handler">Método de conversão de linha da tabela em entidade</param>
        /// <returns>Coleção de objetos da entidade corrente</returns>
        protected IEnumerable<T> ExecuteReader(string procedureName, Func<IDataReader, T> handler)
        {
            return ExecuteReader(procedureName, dr => handler(dr), new { });
        }

        /// <summary>
        /// Executa uma procedure com parâmetros definindo um método de conversão de linha da tabela em entidade customizado
        /// </summary>
        /// <param name="procedureName">Nome da procedure</param>
        /// <param name="handler">Método de conversão de linha da tabela em entidade</param>
        /// <param name="parameters">Instancia de uma classe que terá suas propriedades refletidas como parâmetros da procedure (exceto propriedades marcadas com CampoNaoPersistivelAttribute)
        /// <returns>Coleção de objetos da entidade corrente</returns>
        protected IEnumerable<T> ExecuteReader(string procedureName, Func<IDataReader, T> handler, object parameters)
        {
            Dictionary<String, Object> dictionary = ObjectToDictionary(parameters);
            return ExecuteReader(procedureName, handler, dictionary);
        }

        /// <summary>
        /// Transforma uma instancia de uma classe em um dicionario de parâmetros com nome da propriedade e valor encontrado na instancia
        /// </summary>
        /// <param name="parameters">Instancia com as propriedades a serem transformadas em dicionário</param>
        /// <returns>Dicionário de parâmetros (Nome e valor)</returns>
        private static Dictionary<String, Object> ObjectToDictionary(object parameters)
        {
            Dictionary<String, Object> dictionary = new Dictionary<string, object>();

            if (parameters == null)
            {
                return dictionary;
            }

            if (parameters is Dictionary<string, Object>)
            {
                dictionary = (Dictionary<string, Object>)parameters;
            }
            else
            {
                var parametersType = parameters.GetType();
                var properties = parametersType.GetProperties().Where(p => p.GetCustomAttributes(typeof(CampoNaoPersistivelAttribute), true).Count() == 0);
                foreach (var item in properties)
                {
                    //if (item.GetValue(parameters, null) != null)
                    //{
                    dictionary.Add(item.Name, item.GetValue(parameters, null));
                    //}
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Executa uma procedure com parâmetros definindo um método de conversão de linha da tabela em entidade customizado
        /// </summary>
        /// <param name="procedureName">Nome da procedure</param>
        /// <param name="handler">Método de conversão de linha da tabela em entidade</param>
        /// <param name="parameters">Dicionário contendo os nomes das propriedades e valores a serem passados como parâmetros
        /// <returns>Coleção de objetos da entidade corrente</returns>
        protected IEnumerable<T> ExecuteReader(string procedureName, Func<IDataReader, T> handler, Dictionary<String, object> parameters)
        {

            List<T> list = new List<T>();
            PrepareCommand(procedureName, parameters);

            try
            {
                if (_escopoLocal)
                    _unitOfWork = new UnitOfWork();

                Command.Connection = _unitOfWork.Connection;
                Command.Transaction = _unitOfWork.Transaction;
                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    list.Add(handler(DataReader));
                    if (ItemLoaded != null)
                    {
                        var e = new ItemLoadedEventArgs();
                        ItemLoaded(this, e);
                        if (e.Cancel)
                        {
                            break;
                        }
                        else if (e.Skip)
                        {
                            continue;
                        }
                    }
                }
               

            }
            catch
            {
                ReleaseDatabaseObjects();
                if (_unitOfWork != null)
                _unitOfWork.Rollback();
                throw;
            }
            finally
            {
                ReleaseDatabaseObjects();
                if (_escopoLocal)
                    _unitOfWork.Commit();
            }

            return list;
        }


        /// <summary>
        /// Fecha e Libera os objetos abertos (DataReader e Commad)
        /// </summary>
        private void ReleaseDatabaseObjects()
        {
            if (DataReader != null)
            {
                if (!DataReader.IsClosed)
                    DataReader.Close();

                DataReader.Dispose();
                DataReader = null;
            }

            if (Command != null)
            {
                Command.Dispose();
                Command = null;
            }
        }

        /// <summary>
        /// Configura o Command para executar uma determinada procedure aplicando as regras de convenção nos parâmetros
        /// </summary>
        /// <param name="procedureName">Nome da Procedure</param>
        /// <param name="parameters">Parâmetros (Nome da Propriedade/Valor) sem aplicar as regras de convenção</param>
        private void PrepareCommand(string procedureName, Dictionary<String, object> parameters)
        {
            IDAOFactory factory = ImplementationResolver.ResolveSingleton<IDAOFactory>();
            Command = (factory ?? DAOFactory.GetInstance()).CreateCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = procedureName;
            var mi = Command.Parameters.GetType().GetMethod("AddWithValue");

            foreach (var item in parameters)
            {
                if (item.Value != null)
                {
                    if (item.Value is DateTime)
                    {
                        if (((DateTime)item.Value) < SqlDateTime.MinValue.Value)
                        {
                            mi.Invoke(Command.Parameters, new object[] { string.Concat(DataBaseConventions.InputParametersPrefix, item.Key), SqlDateTime.MinValue.Value });
                            continue;
                        }
                    }

                    mi.Invoke(Command.Parameters, new object[] { string.Concat(DataBaseConventions.InputParametersPrefix, item.Key), item.Value });
                }
                else
                {
                    mi.Invoke(Command.Parameters, new object[] { string.Concat(DataBaseConventions.InputParametersPrefix, item.Key), DBNull.Value });
                }
            }
        }

        /// <summary>
        /// Executa uma procedure sem retorno e sem parâmetros passando um sufixo do nome da procedure
        /// <para>
        /// O Nome da procedure sera composto pelo prefixo de procedures definido nas convenções caso tenha um, o nome da entidade com as regras de convenção aplicadas, e o sufixo passado como parâmetro em procedureIdentifier
        /// </para>
        /// </summary>
        /// <param name="procedureIdentifier">Sufixo do nome da procedure que será executada</param>
        /// <returns></returns>
        protected int ExecuteProcedureFullEntity(string procedureIdentifier)
        {
            return ExecuteNonQueryFullEntity(string.Format("{2}.{1}{0}_{3}", entityNameConvention(typeof(T).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName, procedureIdentifier));
        }

        /// <summary>
        /// Executa uma procedure sem retorno e sem parametros a partir do nome da mesma
        /// </summary>
        /// <param name="procedureName">nome da procedure</param>
        /// <returns></returns>
        private int ExecuteNonQueryFullEntity(string procedureName)
        {
            return ExecuteNonQuery(procedureName, ObjectToDictionary(Activator.CreateInstance<T>()));
        }

        /// <summary>
        /// Executa uma procedure sem retorno e com parâmetros passando um sufixo do nome da procedure
        /// <para>
        /// O Nome da procedure sera composto pelo prefixo de procedures definido nas convenções caso tenha um, o nome da entidade com as regras de convenção aplicadas, e o sufixo passado como parâmetro em procedureIdentifier
        /// </para>
        /// </summary>
        /// <param name="procedureIdentifier">Sufixo do nome da procedure que será executada</param>
        /// <param name="parameters">Instancia de uma classe que terá suas propriedades refletidas como parâmetros da procedure (exceto propriedades marcadas com CampoNaoPersistivelAttribute)</param>
        /// <returns></returns>
        protected int ExecuteProcedure(string procedureIdentifier, object parameters)
        {
            return ExecuteNonQuery(string.Format("{2}.{1}{0}_{3}", entityNameConvention(typeof(T).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName, procedureIdentifier), parameters);
        }

        /// <summary>
        ///  Executa uma procedure que retorna uma coleção de uma entidade passando um sufixo do nome da procedure e parâmetros
        /// </summary>
        /// <param name="procedureIdentifier">Sufixo do nome da procedure que será executada</param>
        /// <param name="parameters">Instancia de uma classe que terá suas propriedades refletidas como parâmetros da procedure (exceto propriedades marcadas com CampoNaoPersistivelAttribute)</param>
        /// <returns></returns>
        protected IEnumerable<T> ExecuteQueryProcedure(string procedureIdentifier, object parameters)
        {
            return ExecuteReader(string.Format("{2}.{1}{0}_{3}", entityNameConvention(typeof(T).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName, procedureIdentifier), parameters);
         }

        /// <summary>
        /// Executa uma procedure sem retorno a partir do nome da mesma e de seus parâmetros
        /// </summary>
        /// <param name="procedureName">Nome da Procedure</param>
        /// <param name="parameters">Instancia de uma classe que terá suas propriedades refletidas como parâmetros da procedure (exceto propriedades marcadas com CampoNaoPersistivelAttribute)</param>
        /// <returns></returns>
        private int ExecuteNonQuery(string procedureName, object parameters)
        {
            return ExecuteNonQuery(procedureName, ObjectToDictionary(parameters));
        }

        /// <summary>
        /// Executa uma procedure sem retorno a partir do nome da procedure e de seus parâmetros
        /// </summary>
        /// <param name="procedureName">Nome da Procedure</param>
        /// <param name="parameters">Dicionário contendo os nomes das propriedades e valores a serem passados como parâmetros</param>
        /// <returns>Resultado numérico da procedure</returns>
        private int ExecuteNonQuery(string procedureName, Dictionary<string, object> parameters)
        {
            int result = 0;
            PrepareCommand(procedureName, parameters);

            try
            {
                if (_escopoLocal)
                    _unitOfWork = new UnitOfWork();

                Command.Connection = _unitOfWork.Connection;
                Command.Transaction = _unitOfWork.Transaction;
                result = Command.ExecuteNonQuery();
                Command.Dispose();
                Command = null;

            
            }
            catch
            {
                ReleaseDatabaseObjects();
                if (_unitOfWork != null)
                _unitOfWork.Rollback();
                throw;
            }
            finally
            {
                ReleaseDatabaseObjects();
                if (_escopoLocal)
                    _unitOfWork.Commit();
            }
            return result;
        }

        /// <summary>
        /// Executa uma procedure seguindo o padrão de nome LISTAR
        /// <para>
        /// Regra para o padrão de nome LISTAR: [Schema].[Prefixo][Nome da Entidade com regras de convenção de nome]_LISTAR_[Sufixo]
        /// </para>
        /// </summary>
        /// <param name="procedureSufix">Sufixo do nome da procedure <example>PorId -> SP_Cliente_LISTAR_PorId</example></param>
        /// <param name="parameters">Instancia de uma classe que terá suas propriedades refletidas como parâmetros da procedure (exceto propriedades marcadas com CampoNaoPersistivelAttribute)</param>
        /// <returns>Coleção da entidade montada a partir do resultado da procedure</returns>
        protected IEnumerable<T> Listar(string procedureSufix, object parameters)
        {
            return ExecuteReader(string.Format("{3}.{2}{0}_LISTAR_{1}", entityNameConvention(typeof(T).Name), procedureSufix, DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName), parameters);
        }

        #region IDAO<T> Members

        /// <summary>
        /// Evento disparado a cada Item carregado em uma lista que está sendo carregada a partir do banco de dados
        /// </summary>
        public event EventHandler<ItemLoadedEventArgs> ItemLoaded;

        /// <summary>
        /// Retorna uma coleção de objetos a partir da tabela toda sem filtro
        /// <para>
        /// A procedure a ser chamada segue o seguinte padrão de nome: [Schema].[Prefixo][Nome da Entidade com regras de convenção de nome]_LISTAR_TODOS
        /// </para>
        /// </summary>
        /// <returns>Toda a tabela em forma de coleção de objetos</returns>
        public IEnumerable<T> Listar()
        {
            return ExecuteReader(string.Format("{2}.{1}{0}_LISTAR_TODOS", entityNameConvention(typeof(T).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName));
        }

        /// <summary>
        /// Executa uma procedure de listagem com um determinado critério de filtro definido por uma classe
        /// <para>
        /// A procedure a ser chamada segue o seguinte padrão de nome: [Schema].[Prefixo][Nome da Entidade com regras de convenção de nome]_LISTAR_[Nome da classe de filtro com regras de convenção de nome]
        /// </para>
        /// </summary>
        /// <typeparam name="TFilter">Classe representando os parâmetros de filtro da procedure</typeparam>
        /// <param name="filtro">Instancia da classe de filtro com os critérios preenchidos nas propriedades</param>
        /// <returns>Coleção de objetos que satisfizeram os critérios do filtro na procedure</returns>
        public IEnumerable<T> Listar<TFilter>(TFilter filtro)
        {
            return ExecuteReader(string.Format("{3}.{2}{0}_LISTAR_{1}", entityNameConvention(typeof(T).Name), entityNameConvention(typeof(TFilter).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName), filtro);
        }

        /// <summary>
        /// Método para recuperar um objeto do banco de dados através de procedure a partir das propriedades definidas como chave da classe
        /// <para>
        /// A procedure a ser chamada segue o seguinte padrão de nome: [Schema].[Prefixo][Nome da Entidade com regras de convenção de nome]_OBTER
        /// </para>
        /// </summary>
        /// <param name="entity">Instancia com as propriedades chave preenchidas</param>
        /// <returns>Instancia totalmente preenchida a partir do banco de dados</returns>
        public virtual T Obter(T entity)
        {
            T obj = Activator.CreateInstance<T>();
            obj = ExecuteReader(string.Format("{2}.{1}{0}_OBTER", entityNameConvention(typeof(T).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName), ObterCamposChave(entity)).FirstOrDefault();
            return obj;
        }

        /// <summary>
        /// Persiste um objeto no banco de dados através de procedure
        /// <para>
        /// A procedure a ser chamada segue o seguinte padrão de nome: [Schema].[Prefixo][Nome da Entidade com regras de convenção de nome]_SALVAR
        /// </para>
        /// </summary>
        /// <param name="entity">Entidade a ser persistida</param>
        /// <returns>Entidade preenchida com atributos gerados automaticamente no caso de inserção</returns>
        public T Salvar(T entity)
        {
            var propriedadesChave = ObterCamposChave(entity);

            ExecuteReader(string.Format("{2}.{1}{0}_SALVAR", entityNameConvention(typeof(T).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName),
            dr =>
            {
                if (propriedadesChave.Count == 1)
                {
                    var pi = entity.GetType().GetProperty(propriedadesChave.Keys.First());
                    pi.SetValue(entity, dr[0], null);
                }
                return entity;
            }
                , entity);

            return entity;
        }

        /// <summary>
        /// Exclui um objeto da base de dados através de procedure
        /// <para>
        /// A procedure a ser chamada segue o seguinte padrão de nome: [Schema].[Prefixo][Nome da Entidade com regras de convenção de nome]_EXCLUIR
        /// </para>
        /// </summary>
        /// <param name="entity">Entidade a ser excluida</param>
        /// <returns>Código númerico da operação</returns>
        public virtual int Excluir(T entity)
        {
            try
            {
                return ExecuteNonQuery(string.Format("{2}.{1}{0}_EXCLUIR", entityNameConvention(typeof(T).Name), DataBaseConventions.StoredProcedurePrefix, DataBaseConventions.SchemaName), ObterCamposChave(entity));
            }
            catch (DbException)
            {
                throw new ApplicationException(string.Format("Não foi possível excluir este(a) {0} pois existem referências vinculadas à este registro", entityNameConvention(typeof(T).Name).ToLower()));
            }

        }

     

     

        #endregion

    }
}


