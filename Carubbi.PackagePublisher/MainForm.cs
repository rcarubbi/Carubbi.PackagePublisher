using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Carubbi.PackagePublisher.Properties;

namespace Carubbi.PackagePublisher
{
    public partial class MainForm : Form
    {
        private const string LAST_USED_FOLDER_KEY = "LastUsedFolder";

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clbPackages.Items.Count; i++)
            {
                if (!clbPackages.GetItemChecked(i)) continue;
                var projectName = clbPackages.Items[i].ToString();
                var entries = Directory.GetFiles(txtPackagesPath.Text, $"{projectName}.csproj",
                    SearchOption.AllDirectories);
                if (entries.Length > 0) Publish(entries[0]);
            }

            MessageBox.Show($@"Publish finished!");
        }

        private void Publish(string projeto)
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtPackagesPath.Text = Settings.Default[LAST_USED_FOLDER_KEY].ToString();
            if (!string.IsNullOrWhiteSpace(txtPackagesPath.Text) && new DirectoryInfo(txtPackagesPath.Text).Exists)
                ListPackages();
        }

        private void ListPackages()
        {
            var projects = Directory.GetFiles(txtPackagesPath.Text, "*.nuspec", SearchOption.AllDirectories);
            clbPackages.Items.Clear();
            foreach (var project in projects) clbPackages.Items.Add(Path.GetFileNameWithoutExtension(project));
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            var result = fdCaminhoBase.ShowDialog();
            if (result != DialogResult.OK) return;
            txtPackagesPath.Text = fdCaminhoBase.SelectedPath;
            Settings.Default[LAST_USED_FOLDER_KEY] = txtPackagesPath.Text;
            Settings.Default.Save();
            ListPackages();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < clbPackages.Items.Count; i++)
                clbPackages.SetItemCheckState(i, chkAll.Checked ? CheckState.Checked : CheckState.Unchecked);
        }
    }
}