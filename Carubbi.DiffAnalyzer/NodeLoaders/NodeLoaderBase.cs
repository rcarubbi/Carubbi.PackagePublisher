using System;
using System.Collections.Generic;

namespace Carubbi.DiffAnalyzer.NodeLoaders
{
    /// <summary>
    /// Responsável por interpretar os valores em um determinado nível na árvore de objetos
    /// <example>
    /// Dadas as classes abaixo:
    /// <code>
    /// class Cliente 
    /// {
    ///     public Endereco EnderecoResidencial
    ///     {
    ///         get;
    ///         set;
    ///     }
    ///     
    ///     public string Nome
    ///     {
    ///         get;
    ///         set;
    ///     }
    ///  }
    ///  
    ///  class Endereco
    ///  {
    ///     public string Logradouro
    ///     {
    ///     get;
    ///     set;
    ///     }
    ///     
    ///     public int? Numero
    ///     {
    ///         get;
    ///         set;
    ///     }
    /// }
    /// </code>
    /// <para>Considere duas instancias de Cliente sendo comparadas. Durante a comparação, os objetos são carregados através de NodeLoaders onde para cada nó da árvore de objetos existe um NodeLoader específico para interpretá-lo.
    /// Cliente -> DefaultNodeLoader
    /// Cliente.Endereco -> DefaultNodeLoader
    /// Cliente.Endereco.Logradouro -> DefaultNodeLoader
    /// Cliente.Endereco.Numero -> NullablePrimitiveNodeLoader
    /// Cliente.Nome -> DefaultNodeLoader
    /// </para>
    /// </example>
    /// </summary>
    public abstract class NodeLoaderBase
    {
        public abstract void LoadNode(Type type, object oldInstance, object newInstance, List<DiffComparison> comparisons);

        protected Action<Type, object, object, List<DiffComparison>> _loadTypeNodeHandler;
       
        public NodeLoaderBase(Action<Type, object, object, List<DiffComparison>> loadTypeNodeHandler)
        {
            _loadTypeNodeHandler = loadTypeNodeHandler;
        }       

        public virtual void LoadNode(string propertyName, object oldInstance, object newInstance, List<DiffComparison> comparisons, int depth)
        {
            throw new NotSupportedException();
        }
    }
}
