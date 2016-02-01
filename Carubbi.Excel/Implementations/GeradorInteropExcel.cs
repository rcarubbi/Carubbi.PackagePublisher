using System;
using System.Collections.Generic;

namespace Carubbi.Excel.Implementations
{
    public class GeradorInteropExcel<TEntidade> : IGeradorPlanilha<TEntidade>
    {
        #region IGeradorPlanilha<TEntidade> Members

        /// <summary>
        /// Gera uma planilha a partir de um template
        /// </summary>
        /// <param name="data"></param>
        /// <param name="caminhoTemplate">caminho do template do excel que será criado</param>
        /// <param name="configuracoes">Configurações do excel que será criado</param>
        /// <returns>Objeto chave(Nome do arquivo) valor(conteúdo do arquivo) correspondente a nova planilha criada</returns>
        public KeyValuePair<string, System.IO.Stream> GerarPlanilha(IEnumerable<TEntidade> data, string caminhoTemplate, ConfigGerador configuracoes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
