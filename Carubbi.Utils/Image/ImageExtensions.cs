using System;
using System.Drawing.Imaging;

namespace Carubbi.Utils.Image
{

    /// <summary>
    /// Extension Methods da classe Image
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Recupera o ImageFormat a partir do RawFormat da imagem
        /// </summary>
        /// <param name="img">Image a ser verificado</param>
        /// <returns>Item do enum ImageFormat</returns>
        public static ImageFormat GetImageFormat(this System.Drawing.Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            else
                return ImageFormat.Wmf;
        }

        /// <summary>
        /// Recupera a extensão padrão do arquivo a partir de seu image format
        /// </summary>
        /// <param name="imageFormat">Enum ImageFormat</param>
        /// <returns>extensão do arquivo</returns>
        public static string GetExtension(this ImageFormat imageFormat)
        {
            if (imageFormat == ImageFormat.Bmp)
            {
                return "bmp";
            }
            else if (imageFormat == ImageFormat.Emf)
            {
                return "emf";
            }
            else if (imageFormat == ImageFormat.Exif)
            {
                return "exif";
            }
            else if (imageFormat == ImageFormat.Gif)
            {
                return "gif";
            }
            else if (imageFormat == ImageFormat.Icon)
            {
                return "ico";
            }
            else if (imageFormat == ImageFormat.Jpeg)
            {
                return "jpg";
            }
            else if (imageFormat == ImageFormat.MemoryBmp)
            {
                return "bmp";
            }
            else if (imageFormat == ImageFormat.Png)
            {
                return "png";
            }
            else if (imageFormat == ImageFormat.Tiff)
            {
                return "tiff";
            }
            else if (imageFormat == ImageFormat.Wmf)
            {
                return "wmf";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Recupera o formato da imagem a partir da extensão
        /// </summary>
        /// <param name="extension">extensão do arquivo</param>
        /// <returns>Objeto ImageFormat</returns>
        public static ImageFormat GetImageFormat(this string extension)
        {
            extension = extension.Trim().ToLower();
            if (extension == "bmp")
            {
                return ImageFormat.Bmp;
            }
            else if (extension == "emf")
            {
                return ImageFormat.Emf;
            }
            else if (extension ==  "exif")
            {
                return ImageFormat.Exif;
            }
            else if (extension == "gif")
            {
                return ImageFormat.Gif;
            }
            else if (extension ==  "ico")
            {
                return ImageFormat.Icon;
            }
            else if (extension == "jpg" || extension == "jpeg")
            {
                return ImageFormat.Jpeg;
            }
            else if (extension == "png")
            {
                return ImageFormat.Png;
            }
            else if (extension == "tiff" || extension == "tif")
            {
                return ImageFormat.Tiff;
            }
            else if (extension == "wmf")
            {
                return ImageFormat.Wmf;
            }
            else
            {
                throw new ArgumentException("extensão desconhecida");
            }
        }


        
    }
}
