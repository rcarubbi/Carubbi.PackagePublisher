using Carubbi.LayoutTemplateEngine.Interfaces;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carubbi.LayoutTemplateEngine.Razor3
{
    public class Razor3Engine : ILayoutTemplateEngine
    {
        public string RenderTemplate(string templateName, IDictionary<string, object> data)
        {
            Engine.Razor.AddTemplate(templateName, templateName);
            // On startup
            Engine.Razor.Compile(templateName, null);
            // instead of the Razor.Parse call
            var result = Engine.Razor.Run(templateName, null, data);
            return result;
        }

        public string RenderTemplate(string masterPage, string templateName, IDictionary<string, object> data)
        {
            Engine.Razor.AddTemplate(masterPage, masterPage);
            return RenderTemplate(templateName, data);
        }
    }
}
