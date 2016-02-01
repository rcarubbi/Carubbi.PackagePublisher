using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carubbi.PDF.Interfaces;
using Aspose.Pdf;
using System.IO;
using Aspose.Pdf.Devices;
using System.Reflection;

namespace Carubbi.PDF.Implementations
{
    /// <summary>
    /// Implementação de conversor de PDF em imagens utilizando Aspose.PDF
    /// </summary>
    public class PDF2ImageAsposePDFConverter : IPDF2ImageConverter
    {
        /// <summary>
        /// Constroi um objeto recuperando e definindo a licença de uso
        /// </summary>
        public PDF2ImageAsposePDFConverter()
        {
            var assembly = Assembly.GetExecutingAssembly();
            License license = new License();
            using (Stream stream = assembly.GetManifestResourceStream("Carubbi.PDF.Assets.Aspose.Total.lic"))
            {
                license.SetLicense(stream);
            }
        }

        /// <summary>
        /// Efetua a conversão do PDF em imagens
        /// </summary>
        /// <param name="pdfPath">Caminho do PDF</param>
        /// <returns>Lista de caminhos das imagens por página</returns>
        public List<string> Convert(string pdfPath)
        {
            List<string> imagePaths = new List<string>();
            Document pdfDocument = new Document(pdfPath);

            for (int pageCount = 1; pageCount <= pdfDocument.Pages.Count; pageCount++)
            {
                var imagePath = Path.Combine(Path.GetDirectoryName(pdfPath), "image" + pageCount + ".jpg");
                using (FileStream imageStream = new FileStream(imagePath, FileMode.Create))
                {
                    // Create Resolution object
                    Resolution resolution = new Resolution(300);
                    // Create JPEG device with specified attributes (Width, Height, Resolution, Quality)
                    // where Quality [0-100], 100 is Maximum
                    JpegDevice jpegDevice = new JpegDevice(resolution, 100);

                    // Convert a particular page and save the image to stream
                    jpegDevice.Process(pdfDocument.Pages[pageCount], imageStream);
                    // Close stream
                    imageStream.Close();
                }


                imagePaths.Add(imagePath);
            }

            return imagePaths;
        }
    }
}
