namespace Statistiks.App
{
    partial class AboutForm
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
            this.pbAppIcon = new System.Windows.Forms.PictureBox();
            this.lbSoftwareName = new System.Windows.Forms.Label();
            this.lbIconAuthor = new System.Windows.Forms.Label();
            this.lbLicense = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbAppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pbAppIcon
            // 
            this.pbAppIcon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbAppIcon.Image = global::Statistiks.App.Properties.Resources.AppIcon;
            this.pbAppIcon.Location = new System.Drawing.Point(18, 12);
            this.pbAppIcon.Name = "pbAppIcon";
            this.pbAppIcon.Size = new System.Drawing.Size(33, 35);
            this.pbAppIcon.TabIndex = 0;
            this.pbAppIcon.TabStop = false;
            // 
            // lbSoftwareName
            // 
            this.lbSoftwareName.AutoSize = true;
            this.lbSoftwareName.Location = new System.Drawing.Point(51, 9);
            this.lbSoftwareName.Name = "lbSoftwareName";
            this.lbSoftwareName.Size = new System.Drawing.Size(49, 13);
            this.lbSoftwareName.TabIndex = 1;
            this.lbSoftwareName.Text = "Statistiks";
            // 
            // lbIconAuthor
            // 
            this.lbIconAuthor.AutoSize = true;
            this.lbIconAuthor.Location = new System.Drawing.Point(51, 34);
            this.lbIconAuthor.Name = "lbIconAuthor";
            this.lbIconAuthor.Size = new System.Drawing.Size(287, 13);
            this.lbIconAuthor.TabIndex = 2;
            this.lbIconAuthor.Text = "App Icon created by Benjamin Humphrey (jigsoaricons.com)";
            // 
            // lbLicense
            // 
            this.lbLicense.AutoSize = true;
            this.lbLicense.Location = new System.Drawing.Point(12, 54);
            this.lbLicense.Name = "lbLicense";
            this.lbLicense.Size = new System.Drawing.Size(273, 39);
            this.lbLicense.TabIndex = 3;
            this.lbLicense.Text = "Published under MS-PL license\r\nProject home page: http://github.com/coirius/Stati" +
    "stiks\r\nIssue tracker: http://github.com/coirius/Statistiks/issues";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 111);
            this.Controls.Add(this.lbLicense);
            this.Controls.Add(this.lbIconAuthor);
            this.Controls.Add(this.lbSoftwareName);
            this.Controls.Add(this.pbAppIcon);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(372, 150);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(372, 150);
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pbAppIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAppIcon;
        private System.Windows.Forms.Label lbSoftwareName;
        private System.Windows.Forms.Label lbIconAuthor;
        private System.Windows.Forms.Label lbLicense;
    }
}