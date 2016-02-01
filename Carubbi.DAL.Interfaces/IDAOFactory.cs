using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Carubbi.DAL.Interfaces
{
    /// <summary>
    /// Interface para a definição de uma fáfrica de comandos e conexões
    /// </summary>
    public interface IDAOFactory
    {
        /// <summary>
        /// Método que retorna um Command
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand();
        
        /// <summary>
        /// Método que retorna uma Connection
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();
    }
}
