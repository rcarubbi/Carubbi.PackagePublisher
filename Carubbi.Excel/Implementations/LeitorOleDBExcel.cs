using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Carubbi.Excel.Implementations
{
    public class LeitorOleDBExcel : ILeitorPlanilha
    {

        private const string XLSX = "Excel 12.0 Xml";
        private const string XLSM = "Excel 12.0 Macro";
        private const string XLSB = "Excel 12.0";
        private const string XLS = "Excel 8.0";
        private const string DATABASE_ENGINE_PROVIDER_2007_OR_SUP = "Microsoft.ACE.OLEDB.12.0";
        private const string DATABASE_ENGINE_PROVIDER_2003 = "Microsoft.Jet.OLEDB.4.0";

        private const string EXCEL_2007_CONNECTION_STRING = @"Provider={2};Data Source={0};Extended Properties='{1};HDR=YES; IMEX=1;'";
        

        #region ILeitorPlanilha Members

        /// <summary>
        /// Método responsável por fazer a leitura da planilha transformando em Objeto
        /// </summary>
        /// <param name="path">Caminho da planilha</param>
        /// <returns>Instância da estrutura de dados que representa os dados da planilha</returns>
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

        /// <summary>
        /// Realiza leitura da planilha a partir de um array de bytes
        /// </summary>
        /// <param name="bytes">array de bytes</param>
        /// <returns>DataSet com os dados da planilha</returns>
        public DataSet LerPlanilhaParaDataSet(byte[] bytes)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, bytes);
            DataSet data = LerPlanilhaParaDataSet(tempFile);
            File.Delete(tempFile);
            return data;
        }

        /// <summary>
        /// Realiza leitura da planilha localizada no caminho informado
        /// </summary>
        /// <param name="path">Caminho da planilha</param>
        /// <returns>DataSet com os dados da planilha</returns>
        public DataSet LerPlanilhaParaDataSet(string path)
        {
            DataSet data = new DataSet();
            using (OleDbConnection excelConnection = new OleDbConnection(string.Format(EXCEL_2007_CONNECTION_STRING, path, ResolveExcelType(path), ResolveDataBaseEngineProvider(path)))) 
            {
                excelConnection.Open();
                var workSheets = GetSchemaTable(excelConnection);
                foreach (DataRow worksheet in workSheets.Rows)
                {
                    string query = "SELECT * FROM [" + worksheet["TABLE_NAME"].ToString() + "]";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, excelConnection);
                    
                    adapter.Fill(data, worksheet["TABLE_NAME"].ToString());
                }
            }

            return data;
        }

      

        #endregion
 
        private DataTable GetSchemaTable(OleDbConnection excelConnection)
        {
            DataTable schemaTable = excelConnection.GetOleDbSchemaTable(
                 OleDbSchemaGuid.Tables,
                 new object[] { null, null, null, "TABLE" });
            return schemaTable;
        }

        private string ResolveDataBaseEngineProvider(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case ".xlsx":
                case ".xlsb":
                case ".xlsm":
                    return DATABASE_ENGINE_PROVIDER_2007_OR_SUP;
                case ".xls":
                    return DATABASE_ENGINE_PROVIDER_2003;
                default:
                    return DATABASE_ENGINE_PROVIDER_2007_OR_SUP;
            }
        }

        private string ResolveExcelType(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case ".xlsx":
                    return XLSX;
                case ".xlsb":
                    return XLSB;
                case ".xlsm":
                    return XLSM;
                case ".xls":
                    return XLS;
                default:
                    return XLSX;
            }
        }
    }
}
