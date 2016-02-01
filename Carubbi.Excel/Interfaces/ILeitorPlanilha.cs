using System.Collections.Generic;
using System.Data;

namespace Carubbi.Excel
{
    /// <summary>
    /// Interface responsável por fazer a leitura da planilha
    /// </summary>
    public interface ILeitorPlanilha
    {
        /// <summary>
        /// Realiza leitura da planilha localizada no caminho informado
        /// </summary>
        /// <param name="path">Caminho da planilha</param>
        /// <returns>Objeto com os dados da planilha</returns>
        PlanilhaResult LerPlanilha(string path);

        /// <summary>
        /// Realiza leitura da planilha a partir de um array de bytes
        /// </summary>
        /// <param name="bytes">array de bytes</param>
        /// <returns>Objeto com os dados da planilha</returns>
        PlanilhaResult LerPlanilha(byte[] bytes);

        /// <summary>
        /// Realiza leitura da planilha a partir de um array de bytes
        /// </summary>
        /// <param name="bytes">array de bytes</param>
        /// <returns>DataSet com os dados da planilha</returns>
        DataSet LerPlanilhaParaDataSet(byte[] bytes);

        /// <summary>
        /// Realiza leitura da planilha localizada no caminho informado
        /// </summary>
        /// <param name="path">Caminho da planilha</param>
        /// <returns>DataSet com os dados da planilha</returns>
        DataSet LerPlanilhaParaDataSet(string path);
    }

    /// <summary>
    /// Estrutura de Dados que representa uma planilha
    /// </summary>
    public class PlanilhaResult
    {
        public PlanilhaResult()
        {
            Abas = new List<object[,]>();
        }

        /// <summary>
        /// Lista das abas com os dados da planilha, cada item da lista é uma matriz bidimensioanl que permite o acesso de cada celula pelas coordenadas x, y
        /// </summary>
        public List<object[,]> Abas { get; set; }
    }


    /// <summary>
    /// Estrutura que representa uma aba da planilha
    /// </summary>
    public class Aba
	{
        public Aba()
        {
            Linhas = new List<string[]>();
        }
        public List<string[]> Linhas { get; set; }
	}
}
