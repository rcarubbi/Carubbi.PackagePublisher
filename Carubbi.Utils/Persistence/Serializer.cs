using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace Carubbi.Utils.Persistence
{
    /// <summary>
    /// Serializador Genérico
    /// </summary>
    /// <typeparam name="T">Tipo a ser serializado/desserializado</typeparam>
    public class Serializer<T>
    {
        /// <summary>
        /// Serializa um objeto em binário para um determinado caminho e arquivo
        /// </summary>
        /// <param name="path">Caminho do arquivo físico onde serão gravados os dados</param>
        /// <param name="instance">Objeto a ser serializado</param>
        public void BinarySerialize(string path, T instance)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(sw.BaseStream, instance);
            }
        }


        /// <summary>
        /// Desserializa um objeto persistido no modo binário
        /// </summary>
        /// <param name="path">Caminho do arquivo binário</param>
        /// <returns>Objeto desserializado</returns>
        public T BinaryDeserialize(string path)
        {
            T instance;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    instance = (T)serializer.Deserialize(sr.BaseStream);
                }
                return instance;
            }
            catch (Exception)
            {
                return default(T);
            }
        }


        /// <summary>
        /// Serializa um objeto em XML
        /// </summary>
        /// <param name="instance">Objeto a ser serializado</param>
        /// <returns>XML gerado</returns>
        public string XmlSerialize(T instance)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.GetEncoding("ISO-8859-1");
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = Environment.NewLine;
            settings.ConformanceLevel = ConformanceLevel.Document;

            using (StringWriter textWriter = new StringWriter())
            {
                XmlWriter writer = XmlWriter.Create(textWriter, settings);
                try
                {
                    serializer.Serialize(writer, instance);
                    return textWriter.ToString();
                }
                finally
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Serializa um objeto em xml
        /// </summary>
        /// <param name="path">Caminho do xml a ser gerado</param>
        /// <param name="instance">Objeto a ser serializado</param>
        public void XmlSerialize(Uri path, T instance)
        { 
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings(); 
            settings.Encoding = Encoding.GetEncoding("ISO-8859-1"); 
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = Environment.NewLine;
            settings.ConformanceLevel = ConformanceLevel.Document;

            using (StreamWriter sw = new StreamWriter(path.AbsolutePath))
            {
                XmlWriter writer = XmlWriter.Create(sw, settings);
                try
                {
                    serializer.Serialize(sw, instance);
                }
                finally
                {
                    writer.Close();
                }
            }
        }


        /// <summary>
        /// Desserializa um objeto a partir de um arquivo xml
        /// </summary>
        /// <param name="path">Caminho do arquivo xml</param>
        /// <returns>Objeto desserializado</returns>
        public T XmlDeserialize(Uri path)
        {
            using (StreamReader sr = new StreamReader(path.AbsolutePath, Encoding.GetEncoding("ISO-8859-1")))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        /// <summary>
        /// Desserializa um objeto a partir do conteudo xml informado
        /// </summary>
        /// <param name="path">Conteúdo XML</param>
        /// <returns>Objeto desserializado</returns>
        public T XmlDeserialize(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }
    }
}
