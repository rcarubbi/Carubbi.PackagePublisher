using Carubbi.PDF.Interfaces;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using Aspose.Pdf.Text.TextOptions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Carubbi.PDF.Implementations
{
    /// <summary>
    /// Implementação de conversor de PDF em texto utilizando Aspose.PDF
    /// </summary>
    public class PDF2TextAsposePDFConverter : IPDF2TextConverter
    {

         /// <summary>
        /// Constroi um objeto recuperando e definindo a licença de uso
        /// </summary>
        public PDF2TextAsposePDFConverter()
        {
            var assembly = Assembly.GetExecutingAssembly();
            License license = new License();
            using (Stream stream = assembly.GetManifestResourceStream("Carubbi.PDF.Assets.Aspose.Total.lic"))
            {
                license.SetLicense(stream);
            }
        }

        /// <summary>
        /// Efetua a conversão do PDF em texto
        /// </summary>
        /// <param name="pdfPath">Caminho do PDF</param>
        /// <returns>Lista de conteudo das páginas</returns>
        public IEnumerable<string> Convert(string pdfPath)
        {
            Document inputDocument = new Document(pdfPath);
            string textoDocumento = string.Empty;

            for (int pageIndex = 1; pageIndex <= inputDocument.Pages.Count; pageIndex++)
            {
                yield return ExtrairPagina(inputDocument, pageIndex); ;
            }
        }

        private string ExtrairPagina(Document inputDocument, int pageIndex)
        {
            var options = new TextExtractionOptions(TextExtractionOptions.TextFormattingMode.Raw);
            TextAbsorber textAbsorber = new TextAbsorber(options);

            string textoPagina = string.Empty;
            using (Document outputDocument = new Document())
            {
                outputDocument.Pages.Add(inputDocument.Pages[pageIndex]);
                outputDocument.Pages.Accept(textAbsorber);
                textoPagina = textAbsorber.Text;
            }
            return textoPagina;
        }
    }
}
