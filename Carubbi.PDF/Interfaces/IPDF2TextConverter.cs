using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.PDF.Interfaces
{
    
    /// <summary>
    /// Interface para Conversor de PDF para texto
    /// </summary>
    public interface IPDF2TextConverter
    {

         /// <summary>
        /// Efetua a conversão do PDF em texto
        /// </summary>
        /// <param name="pdfPath">Caminho do PDF</param>
        /// <returns>Lista de conteudo das páginas</returns>
        IEnumerable<string> Convert(string pdfPath);
    }
}
