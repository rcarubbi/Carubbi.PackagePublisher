using Carubbi.CaptchaBreaker.DTOs;
using Carubbi.CaptchaBreaker.Interfaces;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Carubbi.Extensions;

namespace Carubbi.CaptchaBreaker.Implementation
{
    /// <summary>
    /// Clase responsável por solucionar o captcha
    /// </summary>
    public class ManualCaptchaBreaker : ICaptchaBreaker
    {
        /// <summary>
        /// Método responsavel por realizar a quebra do captcha
        /// </summary>
        /// <param name="image">Byte Array da imagem captcha</param>
        /// <param name="config">Objeto contendo informações relevantes para solucionar o captcha</param>
        /// <returns>String com a solução do captcha</returns>
        public string Break(byte[] image, CaptchaConfig config)
        {
            string tempName = Path.GetTempFileName();
            string textTempFileName = Path.GetFileName(Path.ChangeExtension(tempName, "txt"));
            string extension = (config.TipoImagem ?? ImageFormat.Bmp).GetExtension();
            string imageTempFilename = Path.ChangeExtension(tempName, extension);
            string imageFullPath =string.Format(@"workspace\{0}", Path.GetFileName(imageTempFilename));
            if (!Directory.Exists("workspace"))
                Directory.CreateDirectory("workspace");

            File.WriteAllBytes(imageFullPath, image);
            
            var stream = File.Create(string.Format(@"workspace\{0}", textTempFileName));
            
            stream.Close();
            
            stream.Dispose();

            UI.CaptchaDialog cd = new UI.CaptchaDialog(imageFullPath);
            cd.ShowDialog(); 
           
            bool solutionFound = false;
            
            var conteudo = string.Empty;
            
            bool firstTime = true;
            
            do
            {
                if (!firstTime)
                {
                    Thread.Sleep(5000);
                }
                else
                {
                    firstTime = false;
                }

                conteudo = File.ReadAllText(string.Format(@"workspace\{0}", textTempFileName));

                if (!string.IsNullOrEmpty(conteudo))
                {
                    solutionFound = true;
                }
            } while (!solutionFound);

            return conteudo;
        }
    }
}
