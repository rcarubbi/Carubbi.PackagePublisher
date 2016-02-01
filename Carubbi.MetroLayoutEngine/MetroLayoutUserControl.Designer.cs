namespace Carubbi.MetroLayoutEngine
{
    partial class MetroLayoutUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picVoltar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picVoltar)).BeginInit();
            this.SuspendLayout();
            // 
            // picVoltar
            // 
            this.picVoltar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picVoltar.Image = global::Carubbi.MetroLayoutEngine.Properties.Resources.whiteback;
            this.picVoltar.Location = new System.Drawing.Point(544, 25);
            this.picVoltar.Name = "picVoltar";
            this.picVoltar.Size = new System.Drawing.Size(51, 47);
            this.picVoltar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picVoltar.TabIndex = 0;
            this.picVoltar.TabStop = false;
            this.picVoltar.Click += new System.EventHandler(this.PerformVoltar);
            // 
            // MetroLayoutUserControl
            // 
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.Controls.Add(this.picVoltar);
            this.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MetroLayoutUserControl";
            this.Size = new System.Drawing.Size(598, 462);
            ((System.ComponentModel.ISupportInitialize)(this.picVoltar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.PictureBox picVoltar;

    }
}
