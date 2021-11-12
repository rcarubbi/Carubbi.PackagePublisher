namespace Carubbi.PackagePublisher
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPublish = new System.Windows.Forms.Button();
            this.clbPackages = new System.Windows.Forms.CheckedListBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.txtPackagesPath = new System.Windows.Forms.TextBox();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.lblBasePath = new System.Windows.Forms.Label();
            this.fdCaminhoBase = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnPublish
            // 
            this.btnPublish.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnPublish.Location = new System.Drawing.Point(402, 219);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 22);
            this.btnPublish.TabIndex = 0;
            this.btnPublish.Text = "&Publish";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // clbPackages
            // 
            this.clbPackages.FormattingEnabled = true;
            this.clbPackages.Location = new System.Drawing.Point(21, 42);
            this.clbPackages.Name = "clbPackages";
            this.clbPackages.Size = new System.Drawing.Size(375, 199);
            this.clbPackages.TabIndex = 1;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(24, 17);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(70, 17);
            this.chkAll.TabIndex = 2;
            this.chkAll.Text = "Select All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // txtPackagesPath
            // 
            this.txtPackagesPath.Location = new System.Drawing.Point(158, 16);
            this.txtPackagesPath.Name = "txtPackagesPath";
            this.txtPackagesPath.ReadOnly = true;
            this.txtPackagesPath.Size = new System.Drawing.Size(205, 20);
            this.txtPackagesPath.TabIndex = 3;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSelectPath.Location = new System.Drawing.Point(369, 15);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(27, 22);
            this.btnSelectPath.TabIndex = 4;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // lblBasePath
            // 
            this.lblBasePath.AutoSize = true;
            this.lblBasePath.Location = new System.Drawing.Point(101, 19);
            this.lblBasePath.Name = "lblBasePath";
            this.lblBasePath.Size = new System.Drawing.Size(32, 13);
            this.lblBasePath.TabIndex = 5;
            this.lblBasePath.Text = "Path:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 251);
            this.Controls.Add(this.lblBasePath);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.txtPackagesPath);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.clbPackages);
            this.Controls.Add(this.btnPublish);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "Carubbi Package Publisher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.CheckedListBox clbPackages;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.TextBox txtPackagesPath;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.Label lblBasePath;
        private System.Windows.Forms.FolderBrowserDialog fdCaminhoBase;
    }
}

