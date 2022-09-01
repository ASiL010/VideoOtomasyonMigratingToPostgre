using Npgsql;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VideoOtomasyon
{
    public partial class ŞifreDegistirme : Form
    {

        string connectionString = "Server=localhost;Port=5432;Database=VideoOtomasyon;User Id=postgres;Password=123;";
        NpgsqlConnection baglanti = new NpgsqlConnection();
        NpgsqlCommand komut = new NpgsqlCommand();
        NpgsqlDataReader rdr;
        DataSet ds;

        public ŞifreDegistirme()
        {
            InitializeComponent();

        }

        public static int sonuç = 0;
        private void ŞifreDegistirme_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = connectionString;

            KapçaOluştur(Kapça);


            SifreDegistirenKullanıcı.Text = VideoOtomasyon.KullanıcıADıSession;
        }
        public int birkereSifreDegistireGit = 0;
        private void şifreFormKapat(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                birkereSifreDegistireGit = 2;

                this.Close();

            }
        }


        int Move;
        int Mouse_X;
        int Mouse_Y;

        private void SürükleYukarı(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void SürükleAşşağı(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void SürükleHareket(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void degistirSifreBtn_Click(object sender, EventArgs e)
        {
            if (Fonksiyonlar.injectiondanKoru(yeniSifre1.Text) && Fonksiyonlar.injectiondanKoru(yeniSifre2.Text)
              && Fonksiyonlar.injectiondanKoru(eskiSifre.Text) && Fonksiyonlar.injectiondanKoru(kapcaEnter.Text))
            {
                SQLQuery("Select Sifre from Oturum where Ad='" + VideoOtomasyon.KullanıcıADıSession + "'");
                if (rdr.Read())
                {
                    if (Fonksiyonlar.md5ilesifrele(eskiSifre.Text) == rdr[0].ToString())
                    {

                        if (kapcaEnter.Text ==sonuç.ToString())
                        {
                            if (Fonksiyonlar.SifrelemeKuralları(yeniSifre1.Text) && Fonksiyonlar.SifrelemeKuralları(yeniSifre2.Text))
                            {
                                if (yeniSifre1.Text == yeniSifre2.Text)
                                {
                                    baglanti.Close();
                                    SQLQuery2Parametreli("Update Oturum Set Sifre=@s where Ad=@a", "@s", "@a", Fonksiyonlar.md5ilesifrele(yeniSifre1.Text), VideoOtomasyon.KullanıcıADıSession);
                                    MessageBox.Show("Şifreniz Başarı ile Değiştirilmiştir");
                                    yeniSifre1.Clear();
                                    yeniSifre2.Clear();
                                    eskiSifre.Clear();
                                    kapcaEnter.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Yeni ve Eski Şifreler Aynı değil");
                                }
                            }
                        }
                        else { MessageBox.Show("Captcha Hatalı"); KapçaOluştur(Kapça); kapcaEnter.Clear(); }
                    }
                    else MessageBox.Show("Eski Şifreniz Hatalı");

                }
                baglanti.Close();
            }


        }
        public static string KapçaOluştur(Label s)
        {
            Random rand = new Random();
            int rasgele1 = rand.Next(0, 20);
            int rasgele2 = rand.Next(0, 100);
            sonuç = rasgele1 + rasgele2;
            s.Text = rasgele1.ToString() + " + " + rasgele2.ToString();
            return s.Text;
        }
        private void SQLQuery2Parametreli(string s, string p1, string p2, string d1, string d2)
        {

            komut = new NpgsqlCommand();
            komut.Parameters.AddWithValue(p1, d1);
            komut.Parameters.AddWithValue(p2, d2);
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = s;
            rdr = komut.ExecuteReader();

        }




        private void SQLQuery(string s)
        {
            komut = new NpgsqlCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = s;
            rdr = komut.ExecuteReader();

        }

        private void göstereski_CheckedChanged(object sender, EventArgs e)
        {
            if (göstereski.Checked == true)
            {
                eskiSifre.PasswordChar = '\0';

            }
            if (göstereski.Checked == false)
            {
                eskiSifre.PasswordChar = '*';

            }
        }

        private void gösterY2_CheckedChanged(object sender, EventArgs e)
        {
            if (gösterY2.Checked == true)
            {
                yeniSifre2.PasswordChar = '\0';

            }
            if (gösterY2.Checked == false)
            {
                yeniSifre2.PasswordChar = '*';

            }
        }

        private void gösterY1_CheckedChanged(object sender, EventArgs e)
        {
            if (gösterY1.Checked == true)
            {
                yeniSifre1.PasswordChar = '\0';

            }
            if (gösterY1.Checked == false)
            {
                yeniSifre1.PasswordChar = '*';

            }
        }
    }
}
