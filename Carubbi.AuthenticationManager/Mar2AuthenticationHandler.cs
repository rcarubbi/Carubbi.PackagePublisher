using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Itau.SC.Mar2.Web.Security;
using ItauNS = Itau.SC.Mar2.Web.Security;
namespace Carubbi.AuthenticationManager
{
    /// <summary>
    /// Handler que intercepta a requisição HTTP e envia para o handler do MAR2 para verificar a autenticação na requisição pela lógica do MAR2
    /// </summary>
    public class Mar2AuthenticationHandler: IHttpHandler, IRequiresSessionState
    {
        protected ItauNS.Mar2AuthenticationHandler Handle;

        public Mar2AuthenticationHandler()
        {
            this.Handle = new ItauNS.Mar2AuthenticationHandler();
        }

        #region IHttpHandler Members
        public bool IsReusable
        {
            get { return this.Handle.IsReusable; }
        }

        public void ProcessRequest(HttpContext context)
        {
            this.Handle.ProcessRequest(context);
        }
        #endregion
    }
}
