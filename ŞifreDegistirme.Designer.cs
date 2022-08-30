namespace VideoOtomasyon
{
    partial class ŞifreDegistirme
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
            this.eskiSifre = new System.Windows.Forms.TextBox();
            this.yeniSifre1 = new System.Windows.Forms.TextBox();
            this.yeniSifre2 = new System.Windows.Forms.TextBox();
            this.degistirSifreBtn = new System.Windows.Forms.Button();
            this.kapcaEnter = new System.Windows.Forms.TextBox();
            this.Kapça = new System.Windows.Forms.Label();
            this.göstereski = new System.Windows.Forms.CheckBox();
            this.gösterY1 = new System.Windows.Forms.CheckBox();
            this.gösterY2 = new System.Windows.Forms.CheckBox();
            this.SifreDegistirenKullanıcı = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // eskiSifre
            // 
            this.eskiSifre.Location = new System.Drawing.Point(230, 187);
            this.eskiSifre.Name = "eskiSifre";
            this.eskiSifre.PasswordChar = '*';
            this.eskiSifre.Size = new System.Drawing.Size(100, 22);
            this.eskiSifre.TabIndex = 0;
            // 
            // yeniSifre1
            // 
            this.yeniSifre1.Location = new System.Drawing.Point(230, 248);
            this.yeniSifre1.Name = "yeniSifre1";
            this.yeniSifre1.PasswordChar = '*';
            this.yeniSifre1.Size = new System.Drawing.Size(100, 22);
            this.yeniSifre1.TabIndex = 1;
            // 
            // yeniSifre2
            // 
            this.yeniSifre2.Location = new System.Drawing.Point(230, 297);
            this.yeniSifre2.Name = "yeniSifre2";
            this.yeniSifre2.PasswordChar = '*';
            this.yeniSifre2.Size = new System.Drawing.Size(100, 22);
            this.yeniSifre2.TabIndex = 2;
            // 
            // degistirSifreBtn
            // 
            this.degistirSifreBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.degistirSifreBtn.Location = new System.Drawing.Point(269, 400);
            this.degistirSifreBtn.Name = "degistirSifreBtn";
            this.degistirSifreBtn.Size = new System.Drawing.Size(84, 34);
            this.degistirSifreBtn.TabIndex = 3;
            this.degistirSifreBtn.Text = "İmzala";
            this.degistirSifreBtn.UseVisualStyleBackColor = true;
            this.degistirSifreBtn.Click += new System.EventHandler(this.degistirSifreBtn_Click);
            // 
            // kapcaEnter
            // 
            this.kapcaEnter.Location = new System.Drawing.Point(84, 412);
            this.kapcaEnter.Name = "kapcaEnter";
            this.kapcaEnter.Size = new System.Drawing.Size(108, 22);
            this.kapcaEnter.TabIndex = 4;
            // 
            // Kapça
            // 
            this.Kapça.AutoSize = true;
            this.Kapça.BackColor = System.Drawing.Color.Transparent;
            this.Kapça.Font = new System.Drawing.Font("Times New Roman", 30F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Kapça.Location = new System.Drawing.Point(53, 352);
            this.Kapça.Name = "Kapça";
            this.Kapça.Size = new System.Drawing.Size(100, 57);
            this.Kapça.TabIndex = 6;
            this.Kapça.Text = "000";
            // 
            // göstereski
            // 
            this.göstereski.AutoSize = true;
            this.göstereski.BackColor = System.Drawing.Color.Transparent;
            this.göstereski.Location = new System.Drawing.Point(336, 189);
            this.göstereski.Name = "göstereski";
            this.göstereski.Size = new System.Drawing.Size(18, 17);
            this.göstereski.TabIndex = 7;
            this.göstereski.UseVisualStyleBackColor = false;
            this.göstereski.CheckedChanged += new System.EventHandler(this.göstereski_CheckedChanged);
            // 
            // gösterY1
            // 
            this.gösterY1.AutoSize = true;
            this.gösterY1.BackColor = System.Drawing.Color.Transparent;
            this.gösterY1.Location = new System.Drawing.Point(336, 250);
            this.gösterY1.Name = "gösterY1";
            this.gösterY1.Size = new System.Drawing.Size(18, 17);
            this.gösterY1.TabIndex = 8;
            this.gösterY1.UseVisualStyleBackColor = false;
            this.gösterY1.CheckedChanged += new System.EventHandler(this.gösterY1_CheckedChanged);
            // 
            // gösterY2
            // 
            this.gösterY2.AutoSize = true;
            this.gösterY2.BackColor = System.Drawing.Color.Transparent;
            this.gösterY2.Location = new System.Drawing.Point(336, 299);
            this.gösterY2.Name = "gösterY2";
            this.gösterY2.Size = new System.Drawing.Size(18, 17);
            this.gösterY2.TabIndex = 9;
            this.gösterY2.UseVisualStyleBackColor = false;
            this.gösterY2.CheckedChanged += new System.EventHandler(this.gösterY2_CheckedChanged);
            // 
            // SifreDegistirenKullanıcı
            // 
            this.SifreDegistirenKullanıcı.AutoSize = true;
            this.SifreDegistirenKullanıcı.BackColor = System.Drawing.Color.Transparent;
            this.SifreDegistirenKullanıcı.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.SifreDegistirenKullanıcı.Location = new System.Drawing.Point(77, 119);
            this.SifreDegistirenKullanıcı.Name = "SifreDegistirenKullanıcı";
            this.SifreDegistirenKullanıcı.Size = new System.Drawing.Size(218, 39);
            this.SifreDegistirenKullanıcı.TabIndex = 10;
            this.SifreDegistirenKullanıcı.Text = "Kullanıcı Adı";
            // 
            // ŞifreDegistirme
            // 
            this.AcceptButton = this.degistirSifreBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Olive;
            this.BackgroundImage = global::VideoOtomasyon.Properties.Resources.Şifre_değiştirme_formu_için;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(448, 590);
            this.Controls.Add(this.SifreDegistirenKullanıcı);
            this.Controls.Add(this.gösterY2);
            this.Controls.Add(this.gösterY1);
            this.Controls.Add(this.göstereski);
            this.Controls.Add(this.Kapça);
            this.Controls.Add(this.kapcaEnter);
            this.Controls.Add(this.degistirSifreBtn);
            this.Controls.Add(this.yeniSifre2);
            this.Controls.Add(this.yeniSifre1);
            this.Controls.Add(this.eskiSifre);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(500, 500);
            this.MinimumSize = new System.Drawing.Size(448, 590);
            this.Name = "ŞifreDegistirme";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ŞifreDegistirme";
            this.TransparencyKey = System.Drawing.Color.Olive;
            this.Load += new System.EventHandler(this.ŞifreDegistirme_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.şifreFormKapat);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SürükleAşşağı);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SürükleHareket);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SürükleYukarı);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox eskiSifre;
        private System.Windows.Forms.TextBox yeniSifre1;
        private System.Windows.Forms.TextBox yeniSifre2;
        private System.Windows.Forms.Button degistirSifreBtn;
        private System.Windows.Forms.TextBox kapcaEnter;
        private System.Windows.Forms.Label Kapça;
        private System.Windows.Forms.CheckBox göstereski;
        private System.Windows.Forms.CheckBox gösterY1;
        private System.Windows.Forms.CheckBox gösterY2;
        private System.Windows.Forms.Label SifreDegistirenKullanıcı;
    }
}