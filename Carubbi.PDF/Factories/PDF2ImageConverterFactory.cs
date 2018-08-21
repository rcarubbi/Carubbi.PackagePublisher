using Carubbi.PDF.Interfaces;
using Carubbi.ServiceLocator;

namespace Carubbi.PDF.Factories
{
    /// <summary>
    /// Fábrica de criação de conversor de PDF em imagens
    /// </summary>
    public class PDF2ImageConverterFactory
    {
 
        private static volatile PDF2ImageConverterFactory _instance;
        private static volatile object _locker = new object();


        /// <summary>
        /// Padrão singleton para recuperar a instância
        /// </summary>
        /// <returns></returns>
        public static PDF2ImageConverterFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new PDF2ImageConverterFactory();
                    }
                }
            }
            return _instance;

        }

        /// <summary>
        /// Implementa um conversor a partir do arquivo de configurações de implementação
        /// <seealso cref="ImplementationResolver"/>
        /// </summary>
        /// <returns></returns>
        public IPDF2ImageConverter CreateConverter()
        {
            return ImplementationResolver.Resolve<IPDF2ImageConverter>();
        }

        
    }
}
 