using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Carubbi.PackagePublisher.Properties;

namespace Carubbi.PackagePublisher
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnPublicar_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clbPacotes.Items.Count; i++)
            {
                if (!clbPacotes.GetItemChecked(i)) continue;
                var projectName = clbPacotes.Items[i].ToString();
                var entries = Directory.GetFiles(txtCaminhoPacotes.Text, $"{projectName}.csproj",
                    SearchOption.AllDirectories);
                if (entries.Length > 0) Publicar(entries[0]);
            }

            MessageBox.Show($@"Publicação concluída!");
        }

        private void Publicar(string projeto)
        {
            var fi = new FileInfo(projeto);
            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    FileName = "nuget.exe",
                    RedirectStandardInput = true,
                    Arguments = $"pack \"{fi.FullName}\" -IncludeReferencedProjects -Properties Configuration=Release"
                }
            };
            var lines = new List<string>();
            var errorsLines = new List<string>();
            p.Start();
            while (!p.StandardOutput.EndOfStream) lines.Add(p.StandardOutput.ReadLine());

            while (!p.StandardError.EndOfStream) errorsLines.Add(p.StandardError.ReadLine());


            p.WaitForExit();

            if (errorsLines.Count != 0)
                MessageBox.Show(string.Join(Environment.NewLine, errorsLines), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            txtCaminhoPacotes.Text = Settings.Default["UltimaPastaUtilizada"].ToString();
            if (!string.IsNullOrWhiteSpace(txtCaminhoPacotes.Text) && new DirectoryInfo(txtCaminhoPacotes.Text).Exists)
                ListarPacotes();
        }

        private void ListarPacotes()
        {
            var projects = Directory.GetFiles(txtCaminhoPacotes.Text, "*.nuspec", SearchOption.AllDirectories);
            clbPacotes.Items.Clear();
            foreach (var project in projects) clbPacotes.Items.Add(Path.GetFileNameWithoutExtension(project));
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            var result = fdCaminhoBase.ShowDialog();
            if (result != DialogResult.OK) return;
            txtCaminhoPacotes.Text = fdCaminhoBase.SelectedPath;
            Settings.Default["UltimaPastaUtilizada"] = txtCaminhoPacotes.Text;
            Settings.Default.Save();
            ListarPacotes();
        }

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < clbPacotes.Items.Count; i++)
                clbPacotes.SetItemCheckState(i, chkTodos.Checked ? CheckState.Checked : CheckState.Unchecked);
        }
    }
}