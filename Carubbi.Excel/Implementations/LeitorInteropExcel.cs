using System;

namespace Carubbi.Excel.Implementations
{
    public class LeitorInteropExcel : ILeitorPlanilha
    {
        #region ILeitorPlanilha Members

        /// <summary>
        /// Realiza leitura da planilha localizada no caminho informado
        /// </summary>
        /// <param name="path">Caminho da planilha</param>
        /// <returns>Objeto com os dados da planilha</returns>
        public PlanilhaResult LerPlanilha(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Realiza leitura da planilha a partir de um array de bytes
        /// </summary>
        /// <param name="bytes">array de bytes</param>
        /// <returns>Objeto com os dados da planilha</returns>
        public PlanilhaResult LerPlanilha(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        #endregion


        public System.Data.DataSet LerPlanilhaParaDataSet(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet LerPlanilhaParaDataSet(string path)
        {
            throw new NotImplementedException();
        }
    }
}
