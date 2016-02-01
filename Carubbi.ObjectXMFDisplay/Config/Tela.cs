using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Carubbi.Utils.Data;
using Carubbi.Utils.Persistence;
namespace Carubbi.ObjectXMFDisplay.Config
{
    /// <summary>
    /// Representa uma tela de uma aplicação mainframe
    /// </summary>
    public class Tela
    {
        private string _texto = "";
        private string _textoEditavel = "";
        private List<Navegacao> _navegacao = new List<Navegacao>();
        private BaseMF _configuracoes;
        private int _posicaoX = 0;
        private int _posicaoY = 0;

        public Tela()
        {

        }

        public Tela(string nome, BaseMF configuracoes)
        {
            CarregarTela(nome, configuracoes);
        }


        /// <summary>
        /// Carrega uma tela de acordo com um arquivo .mfc
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="configuracoes"></param>
        public void CarregarTela(string nome, BaseMF configuracoes)
        {
            _configuracoes = configuracoes;
            //Verifica se existe o arquivo da tela
            if (!File.Exists(String.Format("{0}\\{1}.mfc", _configuracoes.CaminhoArquivo, nome)))
            {
                throw new Exception(String.Format("Arquivo de configurações não encontrado para a tela {0}.", nome));
            }
            else
            {
                Tela telaTmp = new Serializer<Tela>().XmlDeserialize(File.ReadAllText(String.Format("{0}\\{1}.mfc", _configuracoes.CaminhoArquivo, nome)).Trim());
                this.Texto = telaTmp.Texto;
                this.TextoEditavel = telaTmp.TextoEditavel;
                this.ItensNavegacao = telaTmp.ItensNavegacao;
            }
        }

        /// <summary>
        /// Exibe o texto informado como parâmetro na tela corrente nas próximas posições editáveis disponíveis
        /// </summary>
        /// <param name="texto">Texto a ser exibido</param>
        public void Escreve(string texto)
        {
            try
            {
                int linha = _posicaoY - 1;
                int coluna = _posicaoX - 1;
                string[] textoEmLinhas = _texto.ToLineArray();
                char[] textoLinha = textoEmLinhas[linha].ToCharArray();
                char[] editavelLinha = _textoEditavel.ToLineArray()[linha].ToCharArray();
                //Se a primeira posição não for editável, o cursor não está em uma posição válida
                if (editavelLinha[coluna] != '@')
                    return;
                //Preenche todas as posições pulando campos não editáveis, não pula linhas
                foreach (char letra in texto.ToCharArray())
                {
                    while (true)
                    {
                        if (editavelLinha[coluna] == '@')
                        {
                            textoLinha[coluna] = letra;
                            coluna++;
                            break;
                        }
                        else
                        {
                            coluna++;
                        }
                    }
                }
                //Atualiza dados
                textoEmLinhas[linha] = new String(textoLinha);
                _texto = String.Join("\n", textoEmLinhas);
            }
            catch { }
        }
        
        /// <summary>
        /// Texto inicial exibido na tela quando esta é carregada
        /// </summary>
        public string Texto { get { return _texto; } set { _texto = value; } }

        /// <summary>
        /// Versão do Texto inicial com a identificação das posições editáveis 
        /// </summary>
        public string TextoEditavel { get { return _textoEditavel; } set { _textoEditavel = value; } }
        
        /// <summary>
        /// Lista com os itens de navegação
        /// </summary>
        public List<Navegacao> ItensNavegacao { get { return _navegacao; } set { _navegacao = value; } }

        /// <summary>
        /// Posição do cursor no eixo X
        /// </summary>
        public int PosicaoX { get { return _posicaoX; } set { _posicaoX = value; } }

        /// <summary>
        /// Posição do cursor no eixo Y
        /// </summary>
        public int PosicaoY { get { return _posicaoY; } set { _posicaoY = value; } }

        /// <summary>
        /// Simula o envio de uma determinada tecla para uma coordenada da tela e efetua as navegações configuradas caso necessário
        /// </summary>
        /// <param name="tecla">Tecla enviada</param>
        /// <param name="cursorPosition">Posição do cursor na tela</param>
        /// <returns>Tela com as alterações sofridas após o envio da tecla</returns>
        public Tela NavegaConsoleKey(ConsoleKey tecla, Point cursorPosition)
        {
            foreach (Navegacao navegacao in _navegacao)
            {
                if (navegacao.VerificaNavegacaoConsoleKey(tecla) 
                    && (navegacao.PosicaoCursorX == cursorPosition.X || navegacao.PosicaoCursorX == 0)
                    && (navegacao.PosicaoCursorY == cursorPosition.Y || navegacao.PosicaoCursorY == 0))
                {
                    return new Tela(navegacao.TelaDestino, _configuracoes);
                }
            }
            return new Tela();
        }
    }
}
