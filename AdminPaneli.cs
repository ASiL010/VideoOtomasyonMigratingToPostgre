using Npgsql;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VideoOtomasyon
{


    public partial class AdminPaneli : Form
    {
        static public string connectionString = "Server=localhost;Port=5432;Database=VideoOtomasyon;User Id=postgres;Password=123;";
        static NpgsqlConnection con = new NpgsqlConnection();
        static NpgsqlCommand cmd = new NpgsqlCommand();
        static NpgsqlDataAdapter da;
        static DataSet ds;
        static NpgsqlDataReader rdr;


        public AdminPaneli()
        {
            InitializeComponent();
        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void oturumToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            pnl_Görüntülemeler.Visible = false;
            pnl_AdminOturum.Visible = true;
            pnl_AdminYorum.Visible = false;
            pnl_AdminIstatistik.Visible = false;

            DataBindingYenileOturum();
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (AdminOturumSifreGöster.Checked == true)
            {
                AdminOturumSifre.PasswordChar = '\0';

            }
            if (AdminOturumSifreGöster.Checked == false)
            {
                AdminOturumSifre.PasswordChar = '*';

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (Fonksiyonlar.SifrelemeKuralları(AdminOturumYeniSifre.Text))
            {
                SQLQuery2Parametreli("Update Oturum Set Sifre=@s where Ad=@a", "@s", "@a", Fonksiyonlar.md5ilesifrele(AdminOturumYeniSifre.Text), AdminOturumAd.Text);
                MessageBox.Show("Şifre Başarı ile Değiştirilmiştir");
                AdminOturumYeniSifre.Clear();
                DataBindingYenileOturum();
            }
            con.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void AdminPaneli_Load(object sender, EventArgs e)
        {
            con.ConnectionString = connectionString;
            AdminOturumYetkilendirme.SelectedIndex = 0;
        }


        private void SQLQuery(string s)
        {
            cmd = new NpgsqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = s;
            rdr = cmd.ExecuteReader();

        }

        private void SQLQuery2Parametreli(string s, string p1, string p2, string d1, string d2)
        {

            cmd = new NpgsqlCommand();
            cmd.Parameters.AddWithValue(p1, d1);
            cmd.Parameters.AddWithValue(p2, d2);
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = s;
            rdr = cmd.ExecuteReader();

        }

        private void SQLQuery1Parametreli(string s, string p1, string d1)
        {

            cmd = new NpgsqlCommand();
            cmd.Parameters.AddWithValue(p1, d1);
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = s;
            rdr = cmd.ExecuteReader();

        }



        private void DataBindingYenileOturum()
        {
            label1.DataBindings.Clear();
            AdminOturumAd.DataBindings.Clear();
            AdminOturumSifre.DataBindings.Clear();
            AdminOturumYetki.DataBindings.Clear();

            con = new NpgsqlConnection(connectionString);
            da = new NpgsqlDataAdapter("Select * from Oturum", connectionString);
            ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();

            kaynak.DataSource = ds.Tables[0];
            kaynakGezgini.BindingSource = kaynak;

            label1.DataBindings.Add(new Binding("Text", kaynak, "id"));
            AdminOturumAd.DataBindings.Add(new Binding("Text", kaynak, "Ad"));
            AdminOturumSifre.DataBindings.Add(new Binding("Text", kaynak, "Sifre"));
            AdminOturumYetki.DataBindings.Add(new Binding("Text", kaynak, "AdminYetkisi"));

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SQLQuery1Parametreli("select Ad from Oturum WHERE Ad = @Adi", "@Adi", AdminOturumAd.Text);

            if (rdr.Read())
            {
                MessageBox.Show("Bu kullanıcı adı zaten var...");
            }
            else
            {
                con.Close();
                SQLQuery2Parametreli("Update Oturum Set Ad=@K where id=@a", "@a", "@K", label1.Text, AdminOturumAd.Text);
                con.Close();
                AdminOturumAd.Clear();
                DataBindingYenileOturum();
                MessageBox.Show("Kullanıcı Adı Başarıyla değiştirildi");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AdminOturumYetkilendirme.SelectedIndex > -1)
            {
                if (AdminOturumYetkilendirme.SelectedIndex == 1)
                {
                    SQLQuery1Parametreli("Update Oturum Set AdminYetkisi='True' where id=@a", "@a", label1.Text);
                    con.Close();
                    MessageBox.Show("Admin yetkisi verildi");
                    AdminOturumYetki.Clear();
                    DataBindingYenileOturum();
                }
                else if (AdminOturumYetkilendirme.SelectedIndex == 2)
                {
                    SQLQuery1Parametreli("Update Oturum Set AdminYetkisi='False' where id=@a", "@a", label1.Text);
                    con.Close();
                    MessageBox.Show("Admin yetkisi Elinden Alındı");
                    AdminOturumYetki.Clear();

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataBindingYenileOturum();
        }

        private void oturumToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataBaseGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboneliklerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            pnl_Görüntülemeler.Visible = true;
            pnl_AdminOturum.Visible = false;
            pnl_AdminYorum.Visible = false;
            pnl_AdminIstatistik.Visible = false;

            string hangisorgu = "Select * from Abonelikler"
                  , hangisorguiledeğişecek = "Select Ad from Oturum inner join Abonelikler on Abonelikler.KimAboneID=Oturum.id where KimAboneID=@k";
            Fonksiyonlar.DataGridRowGüncelleVeSeç(dataGridView1, hangisorgu, hangisorguiledeğişecek, "@k", 1);

            string hangisorguiledeğişecek1 = "Select Ad from Oturum inner join Abonelikler on Abonelikler.KimeAboneID=Oturum.id where KimeAboneID=@k";
            Fonksiyonlar.DataGridRowDeğiştir(dataGridView1, hangisorguiledeğişecek1, "@k", 2);

            string hangisorguiledeğişecek2 = "Select VideoAdı from Videolar inner join Abonelikler on Abonelikler.OlunanVideoID=Videolar.ID where OlunanVideoID=@k";
            Fonksiyonlar.DataGridRowDeğiştir(dataGridView1, hangisorguiledeğişecek2, "@k", 4);


            dataGridView1.Columns[1].HeaderText = "Abone Olanın Adı";
            dataGridView1.Columns[2].HeaderText = "Olunan Kişinin Adı";
            dataGridView1.Columns[4].HeaderText = "VideoAdı";
        }

        private void görüntülenmelerToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            pnl_Görüntülemeler.Visible = true;
            pnl_AdminOturum.Visible = false;
            pnl_AdminYorum.Visible = false;
            pnl_AdminIstatistik.Visible = false;

            string hangisorgu = "Select * from Görüntülenmeler"
                  , hangisorguiledeğişecek = "Select Ad from Oturum inner join Görüntülenmeler on Görüntülenmeler.İzleyenID=Oturum.id where İzleyenID=@k";
            Fonksiyonlar.DataGridRowGüncelleVeSeç(dataGridView1, hangisorgu, hangisorguiledeğişecek, "@k", 1);

            string hangisorguiledeğişecek1 = "Select Ad from Oturum inner join Görüntülenmeler on Görüntülenmeler.İzleyenID=Oturum.id where İzlenenenkisiID=@k";
            Fonksiyonlar.DataGridRowDeğiştir(dataGridView1, hangisorguiledeğişecek1, "@k", 2);

            string hangisorguiledeğişecek2 = "Select VideoAdı from Videolar inner join Görüntülenmeler on Görüntülenmeler.İzlenenenVideoID=Videolar.ID where İzlenenenVideoID=@k";
            Fonksiyonlar.DataGridRowDeğiştir(dataGridView1, hangisorguiledeğişecek2, "@k", 3);


            dataGridView1.Columns[1].HeaderText = "İzleyenin Adı";
            dataGridView1.Columns[2].HeaderText = "İzlenen Kişinin Adı";
            dataGridView1.Columns[3].HeaderText = "VideoAdı";
        }

        private void adminPaneliKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AdminPanelYorumluVideolarListesi.SelectedIndex > -1)
            {
                DataBindingYenileYorumlar();

            }
        }

        private void yorumlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminPanelYorumluVideolarListesi.Items.Clear();
          
            pnl_Görüntülemeler.Visible = false;
            pnl_AdminOturum.Visible = false;
            pnl_AdminYorum.Visible = true;
            pnl_AdminIstatistik.Visible = false;
            AdminVideoListeYenile();
            AdminPanelYorumluVideolarListesi.SelectedIndex = 0;
        }

        private void DataBindingYenileYorumlar()
        {
            Yorumİçeriği.DataBindings.Clear();
            YorumYapanAdı.DataBindings.Clear();
            YorumunTarihi.DataBindings.Clear();
            YorumIDsi.DataBindings.Clear();
            VideoIDsi.DataBindings.Clear();
       
            con = new NpgsqlConnection(connectionString);
            da = new NpgsqlDataAdapter("Select VideoID,Yorumlar.ID,Ad,YorumIcerigi,YorumTarihi from Yorumlar inner join Videolar on Videolar.ID=Yorumlar.VideoID inner join Oturum on Oturum.id=Yorumlar.YorumSahipID where VideoAdı='" + AdminPanelYorumluVideolarListesi.Text + "'", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();

            kaynakYorum.DataSource = ds.Tables[0];
            YorumNavigator.BindingSource = kaynakYorum;

            Yorumİçeriği.DataBindings.Add(new Binding("Text", kaynakYorum, "YorumIcerigi"));
            YorumYapanAdı.DataBindings.Add(new Binding("Text", kaynakYorum, "Ad"));
            YorumunTarihi.DataBindings.Add(new Binding("Text", kaynakYorum, "YorumTarihi"));
            YorumIDsi.DataBindings.Add(new Binding("Text", kaynakYorum, "ID"));

            VideoIDsi.DataBindings.Add(new Binding("Text", kaynakYorum, "VideoID"));


        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (AdminPanelYorumluVideolarListesi.SelectedIndex > -1)
            {
                SQLQuery1Parametreli("select VideoAdı from Videolar where VideoAdı=@v", "@v", VideoAdıDeğiştir.Text);
                if (rdr.Read())
                {
                    MessageBox.Show("Sistemde aynı isimli video zaten var...");
                    con.Close();
                }
                else
                {
                    con.Close();
                    SQLQuery2Parametreli("Update Videolar Set VideoAdı=@a where VideoAdı=@k", "@a", "@k", VideoAdıDeğiştir.Text, AdminPanelYorumluVideolarListesi.Text);
                    con.Close();
                    MessageBox.Show("Videonun İsmi Başarı ile Değiştirildi");
                    VideoAdıDeğiştir.Clear();
                    DataBindingYenileYorumlar();
                    AdminPanelYorumluVideolarListesi.Items.Clear();
                    AdminVideoListeYenile();
                    AdminPanelYorumluVideolarListesi.SelectedIndex = 0;
                }
            }
        }

        private void AdminVideoListeYenile()
        {
            SQLQuery("Select distinct VideoAdı from Videolar inner join Yorumlar on Videolar.ID=Yorumlar.VideoID");
            while (rdr.Read())
            {
                AdminPanelYorumluVideolarListesi.Items.Add(rdr[0].ToString());

            }
            con.Close();

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            SQLQuery2Parametreli("Update Yorumlar Set YorumIcerigi=@a from Yorumlar inner join Videolar on Videolar.ID=Yorumlar.VideoID where Yorumlar.ID=@k", "@a", "@k", Yorumİçeriği.Text, YorumIDsi.Text);
            con.Close();
            MessageBox.Show("Videoya Yapılan Yorum Başarı ile Değiştirildi");
            VideoAdıDeğiştir.Clear();
            DataBindingYenileYorumlar();
            AdminPanelYorumluVideolarListesi.Items.Clear();
            AdminVideoListeYenile();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SQLQuery1Parametreli("Delete From Yorumlar  where Yorumlar.ID=@k", "@k", YorumIDsi.Text);
            con.Close();
            MessageBox.Show("Video Başarı ile Silindi");
            VideoAdıDeğiştir.Clear();
            DataBindingYenileYorumlar();
            AdminPanelYorumluVideolarListesi.Items.Clear();
            AdminVideoListeYenile();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            SQLQuery("Insert into Yorumlar (VideoID,YorumSahipID,YorumIcerigi,YorumTarihi) values ('" + VideoIDsi.Text + "', '" + VideoOtomasyon.KullanıcıIDsiSession + "', '" + Yorumİçeriği.Text + "'" + ",NOW())");
            MessageBox.Show("Yorum Başarı ile Yapıldı");
            VideoAdıDeğiştir.Clear();
            DataBindingYenileYorumlar();
            AdminPanelYorumluVideolarListesi.Items.Clear();
            AdminVideoListeYenile();
        }

        private void videolarToolStripMenuItem_Click(object sender, EventArgs e)
        {
     
            pnl_Görüntülemeler.Visible = true;
            pnl_AdminOturum.Visible = false;
            pnl_AdminYorum.Visible = false;
            pnl_AdminIstatistik.Visible = false;
            string hangisorgu = "Select * from Videolar"
                 , hangisorguiledeğişecek = "Select Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id where SahibiID=@k";
            Fonksiyonlar.DataGridRowGüncelleVeSeç(dataGridView1, hangisorgu, hangisorguiledeğişecek, "@k", 3);
            dataGridView1.Columns[1].HeaderText = "Videonun Adı";
            dataGridView1.Columns[2].HeaderText = "Kayıtlı olduğu yer";
            dataGridView1.Columns[3].HeaderText = "Sahibinin Adı";
            dataGridView1.Columns[4].HeaderText = "Videonun Paylaşılma Durumu";
            dataGridView1.Columns[5].HeaderText = "Videonun Paylaşılma Tarihi";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            VideoOtomasyon a = new VideoOtomasyon();
            a.Show();

        }

        private void UygulamanınİstatistiğiniGörüntüleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (var series in UygulamaChart.Series)
            {
                series.Points.Clear();
            }

       
            pnl_Görüntülemeler.Visible = false;
            pnl_AdminOturum.Visible = false;
            pnl_AdminYorum.Visible = false;
            pnl_AdminIstatistik.Visible = true;
            string a = null, b = null, c = null, d = null, f = null;
            SQLQuery("Select COUNT(id) from Oturum");
            if (rdr.Read()) a = rdr[0].ToString();
            con.Close();
            SQLQuery("Select COUNT(ID) from Videolar");
            if (rdr.Read()) b = rdr[0].ToString();
            con.Close();
            SQLQuery("Select COUNT(ID) from Yorumlar");
            if (rdr.Read()) c = rdr[0].ToString();
            con.Close();
            SQLQuery("Select COUNT(ID) from Görüntülenmeler");
            if (rdr.Read()) d = rdr[0].ToString();
            con.Close();
            SQLQuery("Select COUNT(ID) from Abonelikler");
            if (rdr.Read()) f = rdr[0].ToString();
            con.Close();
            UygulamaChart.Series["Toplam"].Points.Add(double.Parse(a));
            UygulamaChart.Series["Toplam"].Points.Add(double.Parse(b));
            UygulamaChart.Series["Toplam"].Points.Add(double.Parse(c));
            UygulamaChart.Series["Toplam"].Points.Add(double.Parse(d));
            UygulamaChart.Series["Toplam"].Points.Add(double.Parse(f));
            UygulamaChart.Series["Toplam"].Points[0].AxisLabel = "Kullanıcı Sayısı";
            UygulamaChart.Series["Toplam"].Points[1].AxisLabel = "Videoların Sayısı";
            UygulamaChart.Series["Toplam"].Points[2].AxisLabel = "Yorumların Sayısı";
            UygulamaChart.Series["Toplam"].Points[3].AxisLabel = "Görüntülenmelerin Sayısı";
            UygulamaChart.Series["Toplam"].Points[4].AxisLabel = "Aboneliklerin Sayısı";
            UygulamaChart.Series["Toplam"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

        }

        private void YorumlardaAra_TextChanged(object sender, EventArgs e)
        {
            AdminPanelYorumluVideolarListesi.Items.Clear();
            SQLQuery("Select distinct VideoAdı from Videolar inner join Yorumlar on Videolar.ID=Yorumlar.VideoID where VideoAdı Like'%" + YorumlardaAra.Text + "%'");
            while (rdr.Read())
            {
                AdminPanelYorumluVideolarListesi.Items.Add(rdr[0].ToString());

            }
            con.Close();

            AdminPanelYorumluVideolarListesi.SelectedIndex = 0;
        }

        private void oturumRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporPenceresi a =new RaporPenceresi();
            RaporPenceresi.RaporSeç = 0;
            a.ShowDialog();

        }

        private void videoRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporPenceresi a = new RaporPenceresi();
            RaporPenceresi.RaporSeç = 1;
            a.ShowDialog();
        }

        private void görüntülemelerRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporPenceresi a = new RaporPenceresi();
            RaporPenceresi.RaporSeç = 2;
            a.ShowDialog();
        }

        private void yorumlarRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporPenceresi a = new RaporPenceresi();
            RaporPenceresi.RaporSeç = 3;
            a.ShowDialog();
        }

        private void aboneliklerRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporPenceresi a = new RaporPenceresi();
            RaporPenceresi.RaporSeç = 4;
            a.ShowDialog();

        }
    }
}
