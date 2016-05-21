using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.LayoutTemplateEngine.Interfaces
{
    /// <summary>
    /// Gerenciador de Templates
    /// </summary>
    public interface ILayoutTemplateEngine
    {
        /// <summary>
        /// Renderiza um template a partir do nome, substituindo as variáveis do template pelas variáveis passadas como parâmetro
        /// </summary>
        /// <param name="templateName">Nome do Template</param>
        /// <param name="data">Dicionário com as váriáveis (Chave/Valor)</param>
        /// <returns>Template com as variávies substituídas</returns>
        string RenderTemplate(string templateName, IDictionary<string, object> data);

        /// <summary>
        /// Renderiza um template a partir do nome utilizando masterPage, substituindo as variáveis do template pelas variáveis passadas como parâmetro
        /// </summary>
        /// <param name="templateName">Nome da master page</param>
        /// <param name="templateName">Nome do Template</param>
        /// <param name="data">Dicionário com as váriáveis (Chave/Valor)</param>
        /// <returns>Template com as variávies substituídas</returns>
        string RenderTemplate(string masterPage, string templateName, IDictionary<string, object> data);


        string RenderFromContentTemplate(string content, IDictionary<string, object> data);
    }
}
