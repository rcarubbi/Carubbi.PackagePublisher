using System;
using System.IO;
using System.Windows.Forms;
using Carubbi.ObjectXMFDisplay.Config;
using Carubbi.Utils.Persistence;

namespace Carubbi.ObjectXMFDisplay
{
    /// <summary>
    /// Tela de configuração do simulador para indicar a pasta onde se encontram os arquivos .mfc que possuem as telas e as instruções de navegação entre elas
    /// <seealso cref=""/>
    /// </summary>
    public partial class TelaConfiguracoes : Form
    {
        private int _contadorErro = 0;

        public TelaConfiguracoes()
        {
            InitializeComponent();
        }

        public BaseMF Configuracoes { get; set; }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = txtPasta.Text;
            openFileDialog.ShowDialog(this);
            txtPasta.Text = openFileDialog.FileName;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Config_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: Validar se arquivo .mfc é válido
            if (_contadorErro <= 3 && !System.IO.File.Exists(txtPasta.Text))
            {
                MessageBox.Show("Por favor, selecione um arquivo válido de configurações do Mock MF.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                _contadorErro++;
                return;
            }

            if (_contadorErro > 3)
            {
                e.Cancel = false;
                Application.Exit();
                return;
            }

            FileInfo fInfo = new FileInfo(txtPasta.Text);
            Configuracoes.CaminhoArquivo = fInfo.DirectoryName;
            Configuracoes.TelaInicial = Path.GetFileNameWithoutExtension(txtPasta.Text);
            var baseMFSerializer =new Serializer<BaseMF>();
            if (checkSalvar.Checked)
                File.WriteAllText(String.Format("{0}\\{1}.xml", Application.LocalUserAppDataPath, Configuracoes.Nome), baseMFSerializer.XmlSerialize(Configuracoes));
        }

        private void TelaConfiguracoes_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("{0} - {1}", this.Text, Configuracoes.Nome);
        }

       
    }
}
