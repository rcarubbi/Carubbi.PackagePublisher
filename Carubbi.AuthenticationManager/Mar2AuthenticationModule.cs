using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
//using ItauNS = Itau.SC.Mar2.Web.Security;
 
namespace Carubbi.AuthenticationManager
{
    public class Mar2AuthenticationModule : IHttpModule
    {
      //  protected ItauNS.Mar2AuthenticationModule Module;

        public Mar2AuthenticationModule()
        {
         //   this.Module = new ItauNS.Mar2AuthenticationModule();
        }

        public void Dispose()
        {
         //   this.Module.Dispose();
        }

        public void Init(HttpApplication context)
        {
           // context.PreRequestHandlerExecute += new EventHandler(this.RequestHandlerExecute);
        //    this.Module.Init(context);
        }

        private void RequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            if (context != null && context.Request != null && context.Request.Path != null)
            {
                String path = context.Request.Path.ToLower();

                if (path.Contains("login"))
                {
                    //    this.Module.Context_PostReleaseRequestState(sender, e);
                }
                else
                {
                    Boolean isAuthenticated = false;
                    try
                    {
                        if (context.Session != null && context.Session.Count > 0)
                        {
                            //        isAuthenticated = ItauNS.Mar2Identity.IsAuthenticated;
                        }

                    }
                    catch { }

                    Boolean requireAuth = IsPageOrHandler(path) && !IsNonAuthUrl(path);
                    if (!isAuthenticated && requireAuth && path.EndsWith(".aspx"))
                    {
                        //   String redirectUrl = ItauNS.Mar2Config.URL_TelaLoginInicial + "?url=" + HttpUtility.UrlEncode(path + ((context.Request.QueryString.Count > 0) ? "?" + context.Request.QueryString : String.Empty));
                        String redirectUrl = "";
                        context.Response.Redirect(redirectUrl);
                    }
                    else
                    {
                        //     this.Module.Context_PostReleaseRequestState(sender, e);
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se a página requisitada precisa de autenticação
        /// </summary>
        /// <returns>Verdadeiro caso a página requisitada não necessite de autenticação e negativa caso contrário</returns>
        private bool IsNonAuthUrl(string path)
        {
            bool urlLiberada = false;

            if ((path.Contains("mar2")  || path.EndsWith(".ashx") || path.EndsWith(".asmx")) && !path.Contains("public"))
            {
                urlLiberada = true;
            }

            return urlLiberada;
        }

        private bool IsPageOrHandler(string path)
        {
            return path.EndsWith(".ashx") || path.EndsWith(".aspx");
        }

    }
}
