using System;
using System.Data;
using System.IO;
using ExcelDR = Excel;
namespace Carubbi.Excel.Implementations
{
    public class LeitorExcelDataReader : ILeitorPlanilha
    {
        public PlanilhaResult LerPlanilha(string path)
        {
            throw new NotImplementedException();
        }

        public PlanilhaResult LerPlanilha(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public DataSet LerPlanilhaParaDataSet(byte[] bytes)
        {
            var tempFile = Path.GetTempFileName();
            tempFile = Path.ChangeExtension(tempFile, "xlsx");
            File.WriteAllBytes(tempFile, bytes);
            using (var dr = ResolveDataReader(tempFile))
            {
                var ds = dr.AsDataSet();
                SetFirstRowAsHeader(ds);
                return ds;
            }
        }

        public DataSet LerPlanilhaParaDataSet(string path)
        {
            using (var dr = ResolveDataReader(path))
            {
                var ds = dr.AsDataSet();
                SetFirstRowAsHeader(ds);
                return ds;
            }
        }

        private static void SetFirstRowAsHeader(DataSet ds)
        {
            foreach (DataTable table in ds.Tables)
            {
                if (table.Rows.Count == 0) continue;
                foreach (DataColumn dc in table.Columns)
                {
                    string colName = table.Rows[0][dc.Ordinal].ToString().Trim();
                    if (!string.IsNullOrEmpty(colName) && !table.Columns.Contains(colName))
                    {
                        table.Columns[dc.Ordinal].ColumnName = colName;
                    }
                }
                table.Rows[0].Delete();
            }
            ds.AcceptChanges();
        }

        private ExcelDR.IExcelDataReader ResolveDataReader(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case ".xls":
                case ".xlsb":
                    return ExcelDR.ExcelReaderFactory.CreateBinaryReader(File.OpenRead(path));
                case ".xlsx":
                case ".xlsm":
                default:
                    return ExcelDR.ExcelReaderFactory.CreateOpenXmlReader(File.OpenRead(path));
            }
        }
    }
}
