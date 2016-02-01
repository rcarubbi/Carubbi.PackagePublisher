using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.AuthenticationManager
{
    /// <summary>
    /// Estrutura que informa se uma autenticação foi efetuada com sucesso ou não
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Url de redirecionamento após autenticação bem sucedida
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Indica se a autenticação foi feita com sucesso ou não
        /// </summary>
        public bool Success { get; set; }
    }
}
