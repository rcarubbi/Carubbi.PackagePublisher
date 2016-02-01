using System.Collections.Generic;

namespace Carubbi.PDF.Interfaces
{
    /// <summary>
    /// Interface para Conversor de PDF para imagem
    /// </summary>
    public interface IPDF2ImageConverter
    {
        /// <summary>
        /// Converte um pdf em imagens por página
        /// </summary>
        /// <param name="pdfPath">Caminho do PDF</param>
        /// <returns>Lista com os caminhos das imagens</returns>
        List<string> Convert(string pdfPath);
    }
}
