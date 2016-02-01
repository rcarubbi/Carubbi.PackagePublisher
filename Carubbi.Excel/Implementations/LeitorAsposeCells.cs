using System.IO;
using Aspose.Cells;
using System.Linq;
using System.Collections;
using System.Reflection;
using System.Data;
namespace Carubbi.Excel.Implementations
{
    public class LeitorAsposeCells : ILeitorPlanilha
    {

        #region ILeitorPlanilha Members

        /// <summary>
        /// Constroi um leitor com a licença já configurada
        /// </summary>
        public LeitorAsposeCells()
        {
            var assembly = Assembly.GetExecutingAssembly();
            License license = new License();
            using (Stream stream = assembly.GetManifestResourceStream("Carubbi.Excel.Assets.Aspose.Total.lic"))
            {
                license.SetLicense(stream);           
            }
        }

        /// <summary>
        /// Método responsável por fazer a leitura da planilha transformando em Objeto
        /// </summary>
        /// <param name="path">Caminho da planilha</param>
        /// <returns>Instância da estrutura de dados que representa os dados da planilha</returns>
        public PlanilhaResult LerPlanilha(string path)
        {
            PlanilhaResult pr = new PlanilhaResult();
            Workbook wb = new Workbook(path);
            foreach (Worksheet ws in wb.Worksheets)
            {
                pr.Abas.Add(LoadAba(ws));
            }
            
            return pr;
        }


        /// <summary>
        /// Transforma os dados do objeto worksheet em um array bidimensional
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        private object[,] LoadAba(Worksheet ws)
        {
            Aba aba = new Aba();

            object[,] array = ws.Cells.ExportArray(0, 0, ws.Cells.MaxDataRow + 1, ws.Cells.MaxDataColumn + 1);

            return array;
        }
        
        /// <summary>
        /// Ler a planilha e transformar em PlanilhaResult
        /// </summary>
        /// <param name="bytes">Array de bytes</param>
        /// <returns></returns>
        public PlanilhaResult LerPlanilha(byte[] bytes)
        {
            PlanilhaResult pr = new PlanilhaResult();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Workbook wb = new Workbook(ms);
                foreach (Worksheet ws in wb.Worksheets)
                {
                    pr.Abas.Add(LoadAba(ws));
                }
            }

            return pr;
        }


        /// <summary>
        /// Realiza leitura da planilha a partir de um array de bytes
        /// </summary>
        /// <param name="bytes">array de bytes</param>
        /// <returns>DataSet com os dados da planilha</returns>
        public DataSet LerPlanilhaParaDataSet(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                var loadOptions = new LoadOptions(LoadFormat.Xlsx);
                Workbook wb = new Workbook(ms, loadOptions);
                DataSet result = WorkbookToDataSet(wb);
                return result;
            }
        }

        /// <summary>
        /// Realiza leitura da planilha localizada no caminho informado
        /// </summary>
        /// <param name="path">Caminho da planilha</param>
        /// <returns>DataSet com os dados da planilha</returns>
        public DataSet LerPlanilhaParaDataSet(string path)
        {
            var loadOptions = new LoadOptions(ResolveFormat(path));
            Workbook wb = new Workbook(path, loadOptions);
            DataSet result = WorkbookToDataSet(wb);

            return result;
        }

     
        #endregion

        private static DataSet WorkbookToDataSet(Workbook wb)
        {
            var dataSetName = string.IsNullOrWhiteSpace(wb.FileName) ? "MemoryWorkbook" : wb.FileName;
            DataSet result = new DataSet(dataSetName);
            foreach (Worksheet ws in wb.Worksheets)
            {
                DataTable dt = new DataTable(ws.Name);
                dt = ws.Cells.ExportDataTable(0, 0, ws.Cells.Rows.Count, ws.Cells.Columns.Count, new ExportTableOptions{ ExportColumnName= true});
                result.Tables.Add(dt);
            }
            return result;
        }

        private LoadFormat ResolveFormat(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case ".xlsx":
                case ".xlsm":
                    return LoadFormat.Xlsx;
                case ".xlsb":
                    return LoadFormat.Xlsb;
                case ".xls":
                    return LoadFormat.Excel97To2003;
                default:
                    return LoadFormat.Xlsx;
            }
        }
    }
}
