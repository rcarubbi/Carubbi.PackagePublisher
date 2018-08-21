using Carubbi.CaptchaBreaker.Interfaces;
using Carubbi.ServiceLocator;

namespace Carubbi.CaptchaBreaker.Factories
{
    /// <summary>
    /// Fabrica de criação de classes para solucionar captchas
    /// </summary>
    public class CaptchaBreakerFactory
    {
        private CaptchaBreakerFactory()
		{

		}

        private static volatile CaptchaBreakerFactory _instance;
		
        private static volatile object _locker = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CaptchaBreakerFactory GetInstance()
		{
			if (_instance == null)
			{
				lock(_locker)
				{
					if (_instance == null)
					{
                        _instance = new CaptchaBreakerFactory();
					}
				}
			}
			return _instance;
		}

        /// <summary>
        /// Implementa o solucionador de captcha
        /// </summary>
        /// <returns></returns>
        public ICaptchaBreaker CreateCaptchaBreaker()
        {
            return ImplementationResolver.Resolve<ICaptchaBreaker>();
        }

        /// <summary>
        /// Implementa o solucionador de captcha a partir de uma chave alfanumerica
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public ICaptchaBreaker CreateCaptchaBreaker(string chave)
        {
            return (ICaptchaBreaker)ImplementationResolver.Resolve(chave);
        }
    }
}
