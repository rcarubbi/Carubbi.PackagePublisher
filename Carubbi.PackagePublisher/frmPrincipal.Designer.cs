namespace Carubbi.PackagePublisher
{
    partial class frmPrincipal
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
            this.btnPublicar = new System.Windows.Forms.Button();
            this.clbPacotes = new System.Windows.Forms.CheckedListBox();
            this.chkTodos = new System.Windows.Forms.CheckBox();
            this.txtCaminhoPacotes = new System.Windows.Forms.TextBox();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.lblDiretorioBase = new System.Windows.Forms.Label();
            this.fdCaminhoBase = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnPublicar
            // 
            this.btnPublicar.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnPublicar.Location = new System.Drawing.Point(387, 220);
            this.btnPublicar.Name = "btnPublicar";
            this.btnPublicar.Size = new System.Drawing.Size(75, 22);
            this.btnPublicar.TabIndex = 0;
            this.btnPublicar.Text = "&Publicar";
            this.btnPublicar.UseVisualStyleBackColor = true;
            this.btnPublicar.Click += new System.EventHandler(this.btnPublicar_Click);
            // 
            // clbPacotes
            // 
            this.clbPacotes.FormattingEnabled = true;
            this.clbPacotes.Location = new System.Drawing.Point(21, 42);
            this.clbPacotes.Name = "clbPacotes";
            this.clbPacotes.Size = new System.Drawing.Size(360, 199);
            this.clbPacotes.TabIndex = 1;
            // 
            // chkTodos
            // 
            this.chkTodos.AutoSize = true;
            this.chkTodos.Location = new System.Drawing.Point(24, 17);
            this.chkTodos.Name = "chkTodos";
            this.chkTodos.Size = new System.Drawing.Size(56, 17);
            this.chkTodos.TabIndex = 2;
            this.chkTodos.Text = "Todos";
            this.chkTodos.UseVisualStyleBackColor = true;
            this.chkTodos.CheckedChanged += new System.EventHandler(this.chkTodos_CheckedChanged);
            // 
            // txtCaminhoPacotes
            // 
            this.txtCaminhoPacotes.Location = new System.Drawing.Point(143, 16);
            this.txtCaminhoPacotes.Name = "txtCaminhoPacotes";
            this.txtCaminhoPacotes.ReadOnly = true;
            this.txtCaminhoPacotes.Size = new System.Drawing.Size(205, 20);
            this.txtCaminhoPacotes.TabIndex = 3;
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAtualizar.Location = new System.Drawing.Point(354, 15);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(27, 22);
            this.btnAtualizar.TabIndex = 4;
            this.btnAtualizar.Text = "...";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // lblDiretorioBase
            // 
            this.lblDiretorioBase.AutoSize = true;
            this.lblDiretorioBase.Location = new System.Drawing.Point(86, 19);
            this.lblDiretorioBase.Name = "lblDiretorioBase";
            this.lblDiretorioBase.Size = new System.Drawing.Size(51, 13);
            this.lblDiretorioBase.TabIndex = 5;
            this.lblDiretorioBase.Text = "Caminho:";
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 251);
            this.Controls.Add(this.lblDiretorioBase);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.txtCaminhoPacotes);
            this.Controls.Add(this.chkTodos);
            this.Controls.Add(this.clbPacotes);
            this.Controls.Add(this.btnPublicar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPrincipal";
            this.Text = "Carubbi Package Publisher";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPublicar;
        private System.Windows.Forms.CheckedListBox clbPacotes;
        private System.Windows.Forms.CheckBox chkTodos;
        private System.Windows.Forms.TextBox txtCaminhoPacotes;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.Label lblDiretorioBase;
        private System.Windows.Forms.FolderBrowserDialog fdCaminhoBase;
    }
}

