using System;
using System.Collections.Generic;
using System.Data;

namespace Carubbi.DAL
{
    /// <summary>
    /// Interface para criação de uma classe que gerencia a persistencia de dados 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDAO<T>
    {
        /// <summary>
        /// <para>Quando atribuido nulo o escopo da transação passa a ser local para a operação a ser realizada, quando atribuida uma instância,</para>
        /// <para>o escopo passa a ser vinculado a instância do UnitOfWork e o comit é realizado no momento do dispose do UnitOfWork</para>
        /// </summary>
        UnitOfWork UnitOfWork { get; set; }
       
        event EventHandler<ItemLoadedEventArgs> ItemLoaded;
        IEnumerable<T> Listar();
        IEnumerable<T> Listar<TFilter>(TFilter filtro);
        T Obter(T entity);
        T Salvar(T entity);
        int Excluir(T entity);
      
    }
}
