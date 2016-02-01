using System.Collections;

namespace Carubbi.Utils.Configuration
{
    /// <summary>
    /// Classe facilitadora para acessar configurações em seções de arquivos .config baseadas em Chave-Valor (Hashtable)
    ///  <example>
    ///     Acessando um par Chave-Valor de uma seção chamada "NomeSecao" de um arquivo app.config
    ///     <code>
    ///        var configSection = new AppSettings("NomeSecao");
    ///        var valor = configSection["NOME_CHAVE"];
    ///     </code>
    /// </example>
    /// </summary>
    public class AppSettings 
    {
        private Hashtable _hash;

        /// <summary>
        /// Construtor que disponibiliza uma acesso a uma determinada seção
        /// <example>
        ///     Criando uma variavel que permite acesso a pares Chave-Valor de uma seção chamada "NomeSecao" de um arquivo app.config
        ///     <code>
        ///        var configSection = new AppSettings("NomeSecao");
        ///     </code>
        /// </example>
        /// </summary>
        /// <param name="sectionName">nome da seção no arquivo .config da aplicação</param>
        public AppSettings(string sectionName)
        {
            _hash = (Hashtable)System.Configuration.ConfigurationManager.GetSection(sectionName);
        }

        /// <summary>
        /// Método para acesso à um par Chave-Valor dentro de um arquivo .config
        /// <example>
        ///     Exemplo de acesso a um par Chave-Valor dentro de uma seção chamada "NomeSecao" de um arquivo app.config
        ///     <code>
        ///        var valor = configSection["NOME_CHAVE"];
        ///     </code>
        /// </example>
        /// </summary>
        /// <param name="name">Chave para acessar um determinado valor em um arquivo .config</param>
        /// <returns></returns>
        public string this[string name] 
        {
            get
            {
                return _hash[name] != null? _hash[name].ToString() : string.Empty;
            }
        }

    }
}
