using System;
using System.Collections.Generic;
using Carubbi.LayoutTemplateEngine.Interfaces;
using RazorEngine;
using RazorEngine.Templating;

namespace Carubbi.LayoutTemplateEngine.Implementations
{
    /// <summary>
    /// Gerenciador de templates baseado na linguagem Razor
    /// </summary>
    public class RazorEngine : ILayoutTemplateEngine
    {
        public string RenderFromContentTemplate(string content, IDictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Renderiza um template a partir do nome, substituindo as variáveis do template pelas variáveis passadas como parâmetro
        /// </summary>
        /// <param name="templateName">Nome do Template</param>
        /// <param name="data">Dicionário com as váriáveis (Chave/Valor)</param>
        /// <returns>Template com as variávies substituídas</returns>
        public string RenderTemplate(string templateName, IDictionary<string, object> data)
        {
            DynamicViewBag viewBag = new DynamicViewBag();
            viewBag.AddDictionaryValuesEx(data);
            var result = Razor.Parse(templateName, null, viewBag, null);
            return result;
        }

        /// <summary>
        /// Renderiza um template a partir do nome utilizando masterPage, substituindo as variáveis do template pelas variáveis passadas como parâmetro
        /// </summary>
        /// <param name="templateName">Nome da master page</param>
        /// <param name="templateName">Nome do Template</param>
        /// <param name="data">Dicionário com as váriáveis (Chave/Valor)</param>
        /// <returns>Template com as variávies substituídas</returns>
        public string RenderTemplate(string masterPage, string templateName, IDictionary<string, object> data)
        {
            return RenderTemplate(templateName, data);
        }
    }   
}
