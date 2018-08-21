using Carubbi.PackagePublisher.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            for (int i = 0; i < clbPacotes.Items.Count; i++)
            {
                if (clbPacotes.GetItemChecked(i))
                {
                    var projectName = clbPacotes.Items[i].ToString();
                    var entries = Directory.GetFiles(txtCaminhoPacotes.Text, string.Format("{0}.csproj", projectName), SearchOption.AllDirectories);
                    if (entries.Length > 0)
                    {
                        Publicar(entries[0]);
                     
                    }

                }
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
                    Arguments = string.Format("pack \"{0}\" -IncludeReferencedProjects -Properties Configuration=Release", fi.FullName)
                }
            };
            var lines = new List<string>();
            var errorsLines = new List<string>();
            p.Start();
            while (!p.StandardOutput.EndOfStream)
            { 
                lines.Add(p.StandardOutput.ReadLine());
            }

            while (!p.StandardError.EndOfStream)
            {
                errorsLines.Add(p.StandardError.ReadLine());
            }


            p.WaitForExit();

            if (errorsLines.Count == 0)
            {
                var pacote = Directory.GetFiles(Application.StartupPath, "*.nupkg")[0];
                var destino = Path.Combine(@"C:\Users\RaphaelCarubbi\Documents\Components", Path.GetFileName(pacote));
                if (File.Exists(destino))
                    File.Delete(destino);

                File.Copy(pacote, destino);
                File.Delete(pacote);
            }
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            txtCaminhoPacotes.Text = Settings.Default["UltimaPastaUtilizada"].ToString();
            if (!string.IsNullOrWhiteSpace(txtCaminhoPacotes.Text))
                ListarPacotes();
        }

        private void ListarPacotes()
        {
            var projects = Directory.GetFiles(txtCaminhoPacotes.Text, "*.nuspec", SearchOption.AllDirectories);

            foreach (string project in projects)
            {
                clbPacotes.Items.Add(Path.GetFileNameWithoutExtension(project));
            }
        }
        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            var result = fdCaminhoBase.ShowDialog();
            if (result == DialogResult.OK)
            {
          
                txtCaminhoPacotes.Text = fdCaminhoBase.SelectedPath;
                Settings.Default["UltimaPastaUtilizada"] = txtCaminhoPacotes.Text;
                Settings.Default.Save();
                ListarPacotes();
            }
        }

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < clbPacotes.Items.Count; i++)
            {
                clbPacotes.SetItemCheckState(i, chkTodos.Checked ? CheckState.Checked : CheckState.Unchecked);
            }
            
        }
    }
}
