using Carubbi.CaptchaBreaker.DTOs;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace Carubbi.CaptchaBreaker.Interfaces
{
    /// <summary>
    /// Interface resposável por solucionar captcha
    /// </summary>
    [ServiceContract]
    public interface ICaptchaBreaker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        [OperationContract]
        string Break(byte[] image, CaptchaConfig config);
    }
}
