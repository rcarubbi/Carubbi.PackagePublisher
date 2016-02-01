using Carubbi.Utils.IoC;
namespace Carubbi.Excel.Factories
{
    /// <summary>
    /// Fabrica de criação de classes manipuladoras de planilha excel
    /// </summary>
    public class ExcelFactory
    {

        private static volatile ExcelFactory _instance;
        private static volatile object _locker = new object();

        /// <summary>
        /// Padrão singleton para recuperar a instância
        /// </summary>
        /// <returns></returns>
        public static ExcelFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelFactory();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Implementa o leitor a partir do arquivo de configuração
        /// <seealso cref="ImplementationResolver"/>        
        /// </summary>
        /// <returns></returns>
        public ILeitorPlanilha CreateLeitorPlanilha()
        {
            return ImplementationResolver.Resolve<ILeitorPlanilha>();
        }

        /// <summary>
        /// Implementa o gerador a partir do arquivo de configuração
        /// <seealso cref="ImplementationResolver"/>        
        /// </summary>
        /// <returns></returns>
        public IGeradorPlanilha<T> CreateGeradorPlanilha<T>()
        {
            return ImplementationResolver.Resolve<IGeradorPlanilha<T>>();
        }
    }
}
