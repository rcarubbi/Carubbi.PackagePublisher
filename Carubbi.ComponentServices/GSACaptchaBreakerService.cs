using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Carubbi.CaptchaBreaker.Interfaces;
using Carubbi.CaptchaBreaker.Implementation;
using Carubbi.CaptchaBreaker.DTOs;
using System.Drawing.Imaging;

namespace Carubbi.ComponentServices
{
    /// <summary>
    /// Serviço para uso do GSACaptchaBreaker remotamente
    /// </summary>
    public class GSACaptchaBreakerService : ICaptchaBreaker
    {
        public string Break(byte[] image, CaptchaBreaker.DTOs.CaptchaConfig config)
        {
            var cb = new GSACaptchaBreaker();
            
            string result = cb.Break(image, config);
            cb = null;
            return result;
        }
    }
}
