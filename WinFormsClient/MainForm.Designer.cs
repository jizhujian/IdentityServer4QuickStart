namespace WinFormsClient
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RefreshToken = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.CallWebApi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RefreshToken
            // 
            this.RefreshToken.Location = new System.Drawing.Point(25, 20);
            this.RefreshToken.Name = "RefreshToken";
            this.RefreshToken.Size = new System.Drawing.Size(168, 41);
            this.RefreshToken.TabIndex = 0;
            this.RefreshToken.Text = "Refresh Token";
            this.RefreshToken.UseVisualStyleBackColor = true;
            this.RefreshToken.Click += new System.EventHandler(this.RefreshToken_Click);
            // 
            // Output
            // 
            this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output.Location = new System.Drawing.Point(25, 67);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(751, 350);
            this.Output.TabIndex = 1;
            // 
            // CallWebApi
            // 
            this.CallWebApi.Location = new System.Drawing.Point(199, 20);
            this.CallWebApi.Name = "CallWebApi";
            this.CallWebApi.Size = new System.Drawing.Size(168, 41);
            this.CallWebApi.TabIndex = 2;
            this.CallWebApi.Text = "Call WebApi";
            this.CallWebApi.UseVisualStyleBackColor = true;
            this.CallWebApi.Click += new System.EventHandler(this.CallWebApi_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CallWebApi);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.RefreshToken);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button RefreshToken;
        private TextBox Output;
        private Button CallWebApi;
    }
}