namespace Carubbi.ComponentClient
{
    partial class ClientForm
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
            this.btnGsaCaptchaBreakerTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGsaCaptchaBreakerTest
            // 
            this.btnGsaCaptchaBreakerTest.Location = new System.Drawing.Point(12, 12);
            this.btnGsaCaptchaBreakerTest.Name = "btnGsaCaptchaBreakerTest";
            this.btnGsaCaptchaBreakerTest.Size = new System.Drawing.Size(176, 23);
            this.btnGsaCaptchaBreakerTest.TabIndex = 0;
            this.btnGsaCaptchaBreakerTest.Text = "GSA Captcha Breaker Test";
            this.btnGsaCaptchaBreakerTest.UseVisualStyleBackColor = true;
            this.btnGsaCaptchaBreakerTest.Click += new System.EventHandler(this.btnGsaCaptchaBreakerTest_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(194, 262);
            this.Controls.Add(this.btnGsaCaptchaBreakerTest);
            this.Name = "ClientForm";
            this.Text = "Test Client";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGsaCaptchaBreakerTest;
    }
}

