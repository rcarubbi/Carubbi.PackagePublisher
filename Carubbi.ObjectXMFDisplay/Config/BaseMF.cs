using System.Collections.Generic;

namespace Carubbi.ObjectXMFDisplay.Config
{
    /// <summary>
    /// Representa um conjunto de arquivos .mfc que compõem uma aplicação mainframe
    /// </summary>
    public class BaseMF
    {
        string _nome = "";
        string _caminhoArquivo = "";
        string _telaInicial = "";
        List<string> _telas = new List<string>();

        /// <summary>
        /// Nome do arquivo .mfc
        /// </summary>
        public string Nome { get { return _nome; } set { _nome = value; } }

        /// <summary>
        /// Caminho dos arquivos .mfc de uma determinada aplicação
        /// </summary>
        public string CaminhoArquivo { get { return _caminhoArquivo; } set { _caminhoArquivo = value; } }

        /// <summary>
        /// Arquivo .mfc da Tela inicial de uma aplicação
        /// </summary>
        public string TelaInicial { get { return _telaInicial; } set { _telaInicial = value; } }

        /// <summary>
        /// Lista de arquivos .mfc que compõem uma aplicação
        /// </summary>
        public List<string> Telas { get { return _telas; } set { _telas = value; } }
    }
}
