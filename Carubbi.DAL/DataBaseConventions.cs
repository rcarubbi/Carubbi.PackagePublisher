namespace Carubbi.DAL
{
    /// <summary>
    /// Classe responsável por definir convenções de nomes dos objetos de banco de dados 
    /// <example>
    ///     <list type="bullet">
    ///         <item>Prefixo de nome de procedures</item>
    ///         <item>nome de schema</item>
    ///         <item>Prefixo nas colunas de resultados de procedures</item>
    ///         <item>Prefixo nos parâmetros de entrada de procedures</item>
    ///         <item>Chave da ConnectionString no arquivo de configurações</item>
    ///     </list>
    /// </example>
    /// </summary>
    public static class DataBaseConventions
    {
        private static string _connectionStringKey = "Conexao";
        
        private static string _storedProcedurePrefix = "SP_";
        
        private static string _schemaName = "dbo";
        
        /// <summary>
        /// Getter e Setter do método de definição da regra para gerar o nome das tabelas a partir dos nomes das entidades
        /// </summary>
        public static EntityNameConventionHandler EntityNameConventionHandler { get; set; }
     
        public static string ConnectionStringKey { 
            get {
                return _connectionStringKey;
            } 
            set {
                _connectionStringKey = value;
            } 
        }

        public static string StoredProcedurePrefix {
            get {
                return _storedProcedurePrefix;
            }
            set {
                _storedProcedurePrefix = value;
            }
        }

        public static string SchemaName
        {
            get
            {
                return _schemaName;
            }
            set 
            {
                _schemaName = value;
            }
        }

        public static string OutputFieldnamePrefix { get; set; }
        
        public static string InputParametersPrefix { get; set; }
    }

}
