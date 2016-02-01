using System;
using System.Collections.Generic;
using System.IO;

namespace Carubbi.Excel
{
    /// <summary>
    /// Interface responsável por gerar uma planilha
    /// </summary>
    /// <typeparam name="TEntidade"></typeparam>
    public interface IGeradorPlanilha<TEntidade>
    {
        /// <summary>
        /// Gera uma planilha a partir de um template
        /// </summary>
        /// <param name="data"></param>
        /// <param name="caminhoTemplate">caminho do template do excel que será criado</param>
        /// <param name="configuracoes">Configurações do excel que será criado</param>
        /// <returns>Objeto chave(Nome do arquivo) valor(conteúdo do arquivo) correspondente a nova planilha criada</returns>
        KeyValuePair<String, Stream> GerarPlanilha(IEnumerable<TEntidade> data, string caminhoTemplate, ConfigGerador configuracoes);
    }

    /// <summary>
    /// Informações de contexto do evento LinhaAdicionada
    /// </summary>
    /// <typeparam name="TEntidade"></typeparam>
    public class LinhaAdicionadaEventArgs<TEntidade> : EventArgs
    {
        /// <summary>
        /// Indicador para cancelar a inclusão
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Objeto que representa a linha adicionada
        /// </summary>
        TEntidade Linha { get; set; }
    }


    /// <summary>
    /// Define as configurações para criação de planilha
    /// </summary>
    public class ConfigGerador
    {
        public ConfigGerador()
        {
            IndiceAba = 0;
        }

        /// <summary>
        /// Define o indice da aba inicial
        /// </summary>
        public int IndiceAba { get; set; }
        
        /// <summary>
        /// Define se será gerado automaticamente o cabeçalho da planilha
        /// </summary>
        public bool GerarCabecalho { get; set; }
        
        /// <summary>
        /// Define se serão criados os filtros do excel automaticamente
        /// </summary>
        public bool HabilitarFiltros { get; set; }
        
        /// <summary>
        /// Define as colunas serão redimensionadas automaticamente
        /// </summary>
        public bool RedimensionarColunas { get; set; }

        /// <summary>
        /// Nome da planilha
        /// </summary>
        public string NomeArquivo { get; set; }
    }

    /// <summary>
    /// Atributo para configurar um nome amigavel no cabeçario no momento da geração
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class DisplayAttribute : Attribute
    {
        readonly string name;

        /// <summary>
        /// Seta o nome amigável
        /// </summary>
        /// <param name="name"></param>
        public DisplayAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Retorna nome amigável
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }


    }

}
