using System.IO;
using System.Text;
using Carubbi.CaptchaBreaker.DTOs;
using Carubbi.CaptchaBreaker.Interfaces;
using Tesseract;
namespace Carubbi.CaptchaBreaker.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class TesseractCaptchaBreaker : ICaptchaBreaker
    {
        private const string IDIOMA_PORTUGUES = "eng";

        #region ICaptchaBreaker Members

        /// <summary>
        /// Método responsavel por realizar a quebra do captcha
        /// </summary>
        /// <param name="image">Byte Array da imagem captcha</param>
        /// <param name="config">Objeto contendo informações relevantes para solucionar o captcha</param>
        /// <returns>String com a solução do captcha</returns>
        public string Break(byte[] image, CaptchaConfig config)
        {
            var tempfilename = Path.GetTempFileName();
            
            File.WriteAllBytes(tempfilename, image);
            
            var stbResult = new StringBuilder();
            
            using (var engine = new TesseractEngine(ConfigManager.GetInstance().CAMINHO_ARQUIVO_IDIOMA, IDIOMA_PORTUGUES, EngineMode.Default))
            using (var img = Pix.LoadFromFile(tempfilename))
            {
                var i = 1;
                using (var page = engine.Process(img, null))
                {
                    stbResult.AppendLine(page.GetText());
                    if (config.NumeroDePalavras > 1)
                    {
                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();
                            do
                            {
                                if (i % 2 == 0)
                                {
                                    do
                                    {
                                        stbResult.AppendLine(iter.GetText(PageIteratorLevel.Word));
                                    } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                }
                                i++;
                            } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                        }
                    }
                }
            }

            return stbResult.ToString().Replace("\r", string.Empty).Replace("\n", string.Empty); ;
        }

        #endregion
    }
}
