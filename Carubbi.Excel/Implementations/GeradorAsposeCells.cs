using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Cells;
using System.Reflection;

namespace Carubbi.Excel.Implementations
{
    public class GeradorAsposeCells<TEntidade> : IGeradorPlanilha<TEntidade>
    {
        public GeradorAsposeCells()
        {
            var assembly = Assembly.GetExecutingAssembly();
            License license = new License();
            using (Stream stream = assembly.GetManifestResourceStream("Carubbi.Excel.Assets.Aspose.Total.lic"))
            {
                license.SetLicense(stream);
            }
        }

        #region IGeradorPlanilha<TEntidade> Members


        public KeyValuePair<string, System.IO.Stream> GerarPlanilha(IEnumerable<TEntidade> data, string caminhoTemplate, ConfigGerador configuracoes)
        {
            Workbook workbook = new Workbook(caminhoTemplate);
            Worksheet ws = workbook.Worksheets[configuracoes.IndiceAba];

            if (configuracoes.GerarCabecalho)
            {
                GerarCabecalho(ws);
            }


            int numRow = 1;
            foreach (TEntidade item in data)
            {
                var indiceCampo = 0;
                foreach (var field in item.GetType().GetProperties())
                {
                    ws.Cells[numRow, indiceCampo++].PutValue(FormatarValor(field.GetValue(item, null)), true);
                }

                numRow++;
            }

            if (configuracoes.HabilitarFiltros)
            {
                AutoFilter af = ws.AutoFilter;
                af.SetRange(0, 0, 10);
            }

            if (configuracoes.RedimensionarColunas)
            {
                ws.AutoFitColumns();
            }

            
            workbook.FileName = configuracoes.NomeArquivo;
            return ConvertWorkbookToStream(workbook);
        }

        private KeyValuePair<String, Stream> ConvertWorkbookToStream(Workbook wb)
        {
            MemoryStream ms = new MemoryStream();
            wb.Save(ms, GetFormatByExtension(wb.FileName));
            ms.Position = 0;
            return new KeyValuePair<string, Stream>(wb.FileName, ms);
        }

        private SaveFormat GetFormatByExtension(string p)
        {
            switch(Path.GetExtension(p))
            {
                case ".xls":
                    return SaveFormat.Excel97To2003;
                case ".xlsx":
                    return SaveFormat.Xlsx;
                case ".xlsb":
                    return SaveFormat.Xlsb;
                case ".xlsm":
                    return SaveFormat.Xlsm;
                default:
                    return SaveFormat.Auto;
            }
        }

        private string FormatarValor(object valorPuro)
        {
            if (valorPuro == null)
                return string.Empty;

            Type tipoValorPuro = valorPuro.GetType();
            Type tipoValor;
            string valor;

            if (tipoValorPuro.IsGenericType && tipoValorPuro.GetGenericTypeDefinition() == typeof(Nullable<>))
                tipoValor = tipoValorPuro.UnderlyingSystemType;
            else
                tipoValor = tipoValorPuro;

            valor = (valorPuro ?? string.Empty).ToString();

            if (tipoValor == typeof(Int16)
                 || tipoValor == typeof(Int32)
                 || tipoValor == typeof(Int64)
                 || tipoValor == typeof(UInt16)
                 || tipoValor == typeof(UInt64))
            {
                return @"'" + valor;
            }
            else if (valorPuro is DateTime)
            {
                return string.IsNullOrEmpty(valor) ? string.Empty : Convert.ToDateTime(valor).ToShortDateString();
            }
            else
            {
                return valor;
            }
        }

        private static void GerarCabecalho(Worksheet ws)
        {
            var indiceColuna = 0;
            foreach (var pi in typeof(TEntidade).GetProperties())
            {
                string fieldName = pi.Name;
                var displayAttr = pi.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (displayAttr.Length > 0)
                {
                    fieldName = (displayAttr.First() as DisplayAttribute).Name;
                }
                ws.Cells[0, indiceColuna++].PutValue(fieldName);
            }
        }

        #endregion
    }
}
