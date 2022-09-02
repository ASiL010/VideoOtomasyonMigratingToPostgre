using Npgsql;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VideoOtomasyon
{


    public partial class AdminPaneli : Form
    {
       


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
                Fonksiyonlar.paramatrelidata("Update Oturum Set Sifre=@0 where Ad=@1", new[] { Fonksiyonlar.md5ilesifrele(AdminOturumYeniSifre.Text), AdminOturumAd.Text });
                MessageBox.Show("Şifre Başarı ile Değiştirilmiştir");
                AdminOturumYeniSifre.Clear();
                DataBindingYenileOturum();
            }
            Fonksiyonlar.con.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void AdminPaneli_Load(object sender, EventArgs e)
        {
            Fonksiyonlar.con.ConnectionString = Fonksiyonlar.connectionString;
            AdminOturumYetkilendirme.SelectedIndex = 0;
        }


        private void SQLQuery(string s)
        {
            Fonksiyonlar.cmd = new NpgsqlCommand();
            Fonksiyonlar.con.Open();
            Fonksiyonlar.cmd.Connection = Fonksiyonlar.con;
            Fonksiyonlar.cmd.CommandText = s;
            Fonksiyonlar.rdr = Fonksiyonlar.cmd.ExecuteReader();

        }

       


        private void DataBindingYenileOturum()
        {
            label1.DataBindings.Clear();
            AdminOturumAd.DataBindings.Clear();
            AdminOturumSifre.DataBindings.Clear();
            AdminOturumYetki.DataBindings.Clear();

            Fonksiyonlar.con = new NpgsqlConnection(Fonksiyonlar.connectionString);
            Fonksiyonlar.da = new NpgsqlDataAdapter("Select * from Oturum", Fonksiyonlar.connectionString);
            Fonksiyonlar.ds = new DataSet();
            Fonksiyonlar.con.Open();
            Fonksiyonlar.da.Fill(Fonksiyonlar.ds);
            Fonksiyonlar.con.Close();

            kaynak.DataSource = Fonksiyonlar.ds.Tables[0];
            kaynakGezgini.BindingSource = kaynak;

            label1.DataBindings.Add(new Binding("Text", kaynak, "id"));
            AdminOturumAd.DataBindings.Add(new Binding("Text", kaynak, "Ad"));
            AdminOturumSifre.DataBindings.Add(new Binding("Text", kaynak, "Sifre"));
            AdminOturumYetki.DataBindings.Add(new Binding("Text", kaynak, "AdminYetkisi"));

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Fonksiyonlar.paramatrelidata("select Ad from Oturum WHERE Ad = @0",new[] { AdminOturumAd.Text });

            if (Fonksiyonlar.rdr.Read())
            {
                MessageBox.Show("Bu kullanıcı adı zaten var...");
            }
            else
            {
                Fonksiyonlar.con.Close();
                Fonksiyonlar.paramatrelidata("Update Oturum Set Ad=@K where id=@0", new[] { label1.Text, AdminOturumAd.Text });
                Fonksiyonlar.con.Close();
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
                    Fonksiyonlar.paramatrelidata("Update Oturum Set AdminYetkisi=True where id=@0",new[] { label1.Text });
                    Fonksiyonlar.con.Close();
                    MessageBox.Show("Admin yetkisi verildi");
                    AdminOturumYetki.Clear();
                    DataBindingYenileOturum();
                }
                else if (AdminOturumYetkilendirme.SelectedIndex == 2)
                {
                    Fonksiyonlar.paramatrelidata("Update Oturum Set AdminYetkisi='False' where id=@0", new[] { label1.Text });
                    Fonksiyonlar.con.Close();
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

            Fonksiyonlar.con = new NpgsqlConnection(Fonksiyonlar.connectionString);
            Fonksiyonlar.da = new NpgsqlDataAdapter("Select VideoID,Yorumlar.ID,Ad,YorumIcerigi,YorumTarihi from Yorumlar inner join Videolar on Videolar.ID=Yorumlar.VideoID inner join Oturum on Oturum.id=Yorumlar.YorumSahipID where VideoAdı='" + AdminPanelYorumluVideolarListesi.Text + "'", Fonksiyonlar.con);
            Fonksiyonlar.ds = new DataSet();
            Fonksiyonlar.con.Open();
            Fonksiyonlar.da.Fill(Fonksiyonlar.ds);
            Fonksiyonlar.con.Close();

            kaynakYorum.DataSource = Fonksiyonlar.ds.Tables[0];
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
                Fonksiyonlar.paramatrelidata("select VideoAdı from Videolar where VideoAdı=@0", new[] { VideoAdıDeğiştir.Text });
                if (Fonksiyonlar.rdr.Read())
                {
                    MessageBox.Show("Sistemde aynı isimli video zaten var...");
                    Fonksiyonlar.con.Close();
                }
                else
                {
                    Fonksiyonlar.con.Close();
                    Fonksiyonlar.paramatrelidata("Update Videolar Set VideoAdı=@a where VideoAdı=@0",new[] { VideoAdıDeğiştir.Text, AdminPanelYorumluVideolarListesi.Text });
                    Fonksiyonlar.con.Close();
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
            while (Fonksiyonlar.rdr.Read())
            {
                AdminPanelYorumluVideolarListesi.Items.Add(Fonksiyonlar.rdr[0].ToString());

            }
            Fonksiyonlar.con.Close();

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Fonksiyonlar.paramatrelidata("Update Yorumlar Set YorumIcerigi=@0 from  Videolar where Videolar.ID=Yorumlar.VideoID and Yorumlar.ID=@1", new[] { Yorumİçeriği.Text, YorumIDsi.Text });
            Fonksiyonlar.con.Close();
            MessageBox.Show("Videoya Yapılan Yorum Başarı ile Değiştirildi");
            VideoAdıDeğiştir.Clear();
            DataBindingYenileYorumlar();
            AdminPanelYorumluVideolarListesi.Items.Clear();
            AdminVideoListeYenile();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Fonksiyonlar.paramatrelidata("Delete From Yorumlar  where Yorumlar.ID=@0", new[] { YorumIDsi.Text });
            Fonksiyonlar.con.Close();
            MessageBox.Show("Video Başarı ile Silindi");
            VideoAdıDeğiştir.Clear();
            DataBindingYenileYorumlar();
            AdminPanelYorumluVideolarListesi.Items.Clear();
            AdminVideoListeYenile();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            SQLQuery("Insert into Yorumlar (VideoID,YorumSahipID,YorumIcerigi,YorumTarihi) values ('" + VideoIDsi.Text + "', '" + VideoOtomasyon.izleyenkisiid + "', '" + Yorumİçeriği.Text + "'" + ",NOW())");
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
            if (Fonksiyonlar.rdr.Read()) a = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();
            SQLQuery("Select COUNT(ID) from Videolar");
            if (Fonksiyonlar.rdr.Read()) b = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();
            SQLQuery("Select COUNT(ID) from Yorumlar");
            if (Fonksiyonlar.rdr.Read()) c = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();
            SQLQuery("Select COUNT(ID) from Görüntülenmeler");
            if (Fonksiyonlar.rdr.Read()) d = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();
            SQLQuery("Select COUNT(ID) from Abonelikler");
            if (Fonksiyonlar.rdr.Read()) f = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();
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
            while (Fonksiyonlar.rdr.Read())
            {
                AdminPanelYorumluVideolarListesi.Items.Add(Fonksiyonlar.rdr[0].ToString());

            }
            Fonksiyonlar.con.Close();

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
