using System;
using System.Collections.Generic;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Classe que compara objetos por reflexão a partir de propriedades configuradas no construtor ou no setter SortClasses 
    /// <para>Utilizado em métodos de ordenação em coleção de objetos complexos que requerem uma implementação de IComparer<T> como por exemplo Sort()</para>
    /// <example>
    /// List<SortClass> propriedades = new List<SortClass>();
    /// propriedades.Add(new SortClass("Propriedade1", SortDirection.Ascending));
    /// propriedades.Add(new SortClass("Propriedade2", SortDirection.Descending));
    /// GenericMultipleComparer<Tabela> comparador = new GenericMultipleComparer<Tabela>(propriedades); </example>
    /// </summary>
    /// <typeparam name="T">Tipo da classe a ser comparada</typeparam>
    public class GenericMultipleComparer<T> : System.Collections.Generic.IComparer<T> {
         
       private List<SortClass> _sortClasses;

       /// <summary>
       /// Coleção de Metadados de propriedades através das quais será feita a ordenação
       /// </summary>
       public List<SortClass> SortClasses {
           get { return _sortClasses; }
       }

       /// <summary>
       /// Costrutor Padrão
       /// </summary>
       public GenericMultipleComparer()
       {
           _sortClasses = new List<SortClass>();
       }

       /// <summary>
       /// Construtor que recebe uma coleção de metadados de propriedades de ordeção
       /// </summary>
       /// <param name="SortClasses">lista de metadados</param>
       public GenericMultipleComparer(List<SortClass> SortClasses)
       {
           _sortClasses = SortClasses;
       }

       /// <summary>
       /// Construtor que recebe metadados de apenas uma propriedade
       /// </summary>
       /// <param name="SortColumn">Nome da propriedade</param>
       /// <param name="SortDirection">Direção da ordenação</param>
       public GenericMultipleComparer(string SortColumn, SortDirection SortDirection)
       {
           _sortClasses = new List<SortClass>();
           _sortClasses.Add(new SortClass(SortColumn, SortDirection));
       }
       
       /// <summary>
       /// Comparação de dois objetos através de reflexão a partir dos metadados das propriedades configuradas
       /// </summary>
       /// <param name="x">Object 1</param>
       /// <param name="y">Object 2</param>
       /// <returns></returns>
       public int Compare(T x, T y) {
           if(SortClasses.Count == 0) {
               return 0;
           }
           return CheckSort(0, x, y);
       }

       /// <summary>
       /// Método de ordenação recursivo
       /// </summary>
       /// <param name="SortLevel">Nível na pilha de chamadas</param>
       /// <param name="MyObject1">Objeto 1</param>
       /// <param name="MyObject2">Objeto 2</param>
       /// <returns></returns>
       private int CheckSort(int SortLevel, T MyObject1, T MyObject2) {
           int returnVal = 0;
           
           if(SortClasses.Count - 1 >= SortLevel) {
               object valueOf1 = MyObject1.GetType().GetProperty(SortClasses[SortLevel].SortColumn).GetValue(MyObject1, null);
               object valueOf2 = MyObject2.GetType().GetProperty(SortClasses[SortLevel].SortColumn).GetValue(MyObject2, null);

               if(SortClasses[SortLevel].SortDirection == SortDirection.Ascending) {
                   returnVal = ((IComparable) valueOf1).CompareTo(valueOf2);
               } 
               else {
                   returnVal = ((IComparable) valueOf2).CompareTo(valueOf1);
               }

               if(returnVal == 0){
                   returnVal = CheckSort(SortLevel + 1, MyObject1, MyObject2);
               }
           }
           return returnVal;
       }
   }

   /// <summary>
   /// Enumerador que determina a direção da ordenação
   /// </summary>
    public enum SortDirection
    {
       /// <summary>Sort Ascending</summary>
       Ascending = 1,

       /// <summary>Sort Descending</summary>
       Descending = 2
   }

   /// <summary>
    /// Estrutura de dados que representa os metadados de uma propriedade no contexto da ordenação
    /// </summary>
    public class SortClass
    {
        /// <summary>
        /// Construtor padrão que recebe o nome da propriedade e a direção da ordenação
        /// </summary>
        /// <param name="SortColumn">Nome da Coluna a ser ordenada</param>
        /// <param name="SortDirection">Direção da Ordenação</param>
        public SortClass(string SortColumn, SortDirection SortDirection)
        {
            this.SortColumn = SortColumn;
            this.SortDirection = SortDirection;
        }

        private string    _sortColumn;
        
        /// <summary>
        /// The column to sort on
        /// </summary>
        public string SortColumn
        {
            get { return _sortColumn; }
            set { _sortColumn = value; }
        }
        
        private SortDirection _sortDirection;

        /// <summary>
        /// The direction to sort
        /// </summary>
        public SortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }
    }
    
}
