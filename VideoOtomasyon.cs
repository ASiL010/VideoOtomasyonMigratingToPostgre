using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VideoOtomasyon
{
    public partial class VideoOtomasyon : Form
    {
        //OguzDTO.mdf ve OguzDTO.ldf isimli olan dosyaları SQL servere attach yapmadan önce güvenlik ilkelerinden bu dosyalar için  bütün erişim izinlerinin alınması gerekmektedir.
        //Program derlenmeden önce VideoOtomasyon.cs , Fonksiyonlar.cs , AdminPaneli.cs , ŞifreDegistirme.cs dosyalarındaki "connectionString"-lerinin güncellenmesi gerekmektedir.
        string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=OguzDTO;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection baglanti = new SqlConnection();
        SqlCommand komut = new SqlCommand();
        SqlDataReader rdr;

        public VideoOtomasyon()
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = connectionString;


        }

        public void AbonelikChartDoldur(string yeri)
        {
            AboneChart.Series["Toplam"].Points.Add(double.Parse(Fonksiyonlar.SayisiniGetir(" Select COUNT(SahibiID) from Abonelikler Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID where KimAboneID=@k", "@k", yeri)));
            AboneChart.Series["Toplam"].Points.Add(double.Parse(Fonksiyonlar.SayisiniGetir(" Select COUNT(KimAboneID) from Abonelikler Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID   where KimeAboneID=@i", "@i", yeri)));
            AboneChart.Series["Toplam"].Points[0].AxisLabel = "Abone Olunanlar";
            AboneChart.Series["Toplam"].Points[1].AxisLabel = "Abone Olanlar";
            AboneChart.Series["Toplam"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

        }

        private void Abonelikler_clcked(object sender, EventArgs e)
        {
            pnl_Anasayfa.Visible = false;
            pnl_Profil.Visible = false;
            pnl_abonelikler.Visible = true;
            VideoİzleyeniBelirle();

            Fonksiyonlar.DataGridRowGüncelle(aboneolgmkanal, "select SahibiID,AboneOlmaTarihi,VideoAdı,VideoTarihi from Abonelikler  Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID   where KimAboneID='" + izleyenkisiid + "'", "Select Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id inner join Abonelikler on Abonelikler.KimAboneID=Oturum.id where KimAboneID=@k", "@k");
            aboneolgmkanal.Columns[0].HeaderText = "Abone Olduğunuz Kanalın İsmi";
            aboneolgmkanal.Columns[1].HeaderText = "Abone Olduğunuz Tarih";
            aboneolgmkanal.Columns[2].HeaderText = "Abone Olduğunuz Videosu";
            aboneolgmkanal.Columns[3].HeaderText = "Videonun Paylaşılma Tarihi";
            Fonksiyonlar.DataGridRowGüncelle(aboneolankanal, "select KimAboneID,AboneOlmaTarihi,VideoAdı,VideoTarihi from Abonelikler Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID   where KimeAboneID='" + izleyenkisiid + "'", "Select Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id inner join Abonelikler on Abonelikler.KimAboneID=Oturum.id where KimAboneID=@k", "@k");
            aboneolankanal.Columns[0].HeaderText = "Abone Olan Kanalın İsmi";
            aboneolankanal.Columns[1].HeaderText = "Size Abone Olduğu Tarih";
            aboneolankanal.Columns[2].HeaderText = "Videonuzun İsmi";
            aboneolankanal.Columns[3].HeaderText = "Videoyuyu Paylaştığınız Tarih";

            ChartTemizle(AboneChart);
            AbonelikChartDoldur(izleyenkisiid);
        }
        private void btn_Anasayfa_Click(object sender, EventArgs e)
        {
            pnl_Anasayfa.Visible = true;
            pnl_Profil.Visible = false;
            pnl_abonelikler.Visible = false;


            ListeleriYenile();
        }

        private void btn_Profil_Click(object sender, EventArgs e)
        {
            pnl_Anasayfa.Visible = false;
            pnl_Profil.Visible = true;
            pnl_abonelikler.Visible = false;
            wmp.Ctlcontrols.stop();
            ListeleriYenile();
        }

        private void btn_HesapOlustur_Click(object sender, EventArgs e)
        {

            try
            {
                if (Fonksiyonlar.injectiondanKoru(txt_Kullanici1.Text) && Fonksiyonlar.injectiondanKoru(txt_Sifre1.Text))
                {
                    if (txt_Kullanici1.TextLength <= 10 && txt_Sifre1.TextLength <= 10)
                    {
                        if (Fonksiyonlar.SifrelemeKuralları(txt_Sifre1.Text))
                        {

                            SQLQuery1Parametreli("select id,Ad from Oturum WHERE Ad = @Adi", "@Adi", txt_Kullanici1.Text);

                            if (rdr.Read())//sqli okursa kullanıcı ve şifresi doğrudur
                            {
                                MessageBox.Show("Bu kullanıcı adı zaten var...");
                            }
                            else
                            {
                                string girissifresi = Fonksiyonlar.md5ilesifrele(txt_Sifre1.Text);
                                baglanti.Close();
                                SQLQuery("insert Oturum (Ad,Sifre,AdminYetkisi,SonGiris) values ('" + txt_Kullanici1.Text + "','" + girissifresi + "','" + 0 + "'" + ",GETDATE() )");
                                baglanti.Close();
                                MessageBox.Show("Kullanıcı Başarıyla Kaydedildi.");
                            }
                            baglanti.Close();
                        }
                    }
                    else MessageBox.Show("Kullanıcı isminizin veya Şifrenizin uzunluğu 10'dan fazla olamaz");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static string KullanıcıIDsiSession = "";
        public static string KullanıcıADıSession = "";
        private void btn_GirisYap_Click(object sender, EventArgs e)
        {
            if (
            Fonksiyonlar.injectiondanKoru(txt_Kullanici2.Text) &&
           Fonksiyonlar.injectiondanKoru(txt_Sifre2.Text))
            {
                if (txt_Kullanici1.TextLength <= 10 && txt_Sifre1.TextLength <= 10)
                {
                    //şifre md5 ile şifrelenip sqle kayıt edilirken normal uzunluğu ile kayıt edilirken problem çıkmıştır bu problem şifre nvarchar(100) yapıldığında ortadan kalmıştır.
                    SQLQuery("select id,Ad from Oturum WHERE Ad = '" + txt_Kullanici2.Text + "' AND Sifre = '" + Fonksiyonlar.md5ilesifrele(txt_Sifre2.Text) + "'");
                    if (rdr.Read())//sqli okursa kullanıcı ve şifresi doğrudur
                    {
                        KullanıcıIDsiSession = rdr["id"].ToString();
                        KullanıcıADıSession = rdr["Ad"].ToString();
                        MessageBox.Show("Giriş Başarılı");
                        lbl_Kullanici.Text = rdr["Ad"].ToString();

                        baglanti.Close();
                        SQLQuery1Parametreli("UPDATE Oturum SET SonGiris=GETDATE() WHERE Ad=@kullanicininadi", "@kullanicininadi", txt_Kullanici2.Text);

                        pnl_Giris.Visible = false;

                        if (Fonksiyonlar.YetkiKontrol()) { btn_AdmineGit.Visible = true; btn_AdmineGit.Enabled = true; }

                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifresi yanlış");

                    }
                    baglanti.Close();



                    baglanti.Close();
                    ListeleriYenile();
                }
            }
            else MessageBox.Show("Kullanıcı isminizin veya Şifrenizin uzunluğu 10'dan fazla olamaz");

        }

        private void btn_AdmineGit_Click(object sender, EventArgs e)
        {
            AdminPaneli adminpanel = new AdminPaneli();
            adminpanel.ShowDialog();
        }

        ŞifreDegistirme fr = new ŞifreDegistirme();
        private void SifreDegistireGit(object sender, EventArgs e)
        {

            if (fr.birkereSifreDegistireGit == 2) { fr = new ŞifreDegistirme(); fr.Show(); }
            else fr.Show();

        }


        private void MediaError(object sender, EventArgs e)
        {
            MessageBox.Show("Bu media Çalınamaz.");
        }
        string VideonunID = null, VideoSahibiID = null, izleyenkisiid = null;
        private void listIndexDegisti(object sender, EventArgs e)
        {
            if (list_Videolar.SelectedIndex > -1)
            {
                int listeyetıklamasayisi = 0;
                if (listeyetıklamasayisi == 0)
                {
                    btn_Begen.Enabled = true;
                    btn_begenme.Enabled = true;
                    Abone_ol_Btn.Enabled = true;
                    btn_Yorum.Enabled = true;
                    listeyetıklamasayisi++;
                }


                VideoİzleyeniBelirle();

                SQLQuery1Parametreli("select id,Videolar.ID,SahibiID,Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id where VideoAdı=@videoadi", "@videoadi", list_Videolar.Text);
                if (rdr.Read())
                {
                    //Hangi kanalın videosunun izlendiğinin gösterilmesi
                    lbl_izlenenKanal.Text = rdr["Ad"].ToString(); //izlenen kanalın adını yazmak
                    if (rdr["Ad"].ToString() == lbl_Kullanici.Text) Abone_ol_Btn.Enabled = false;  //kişi kendisine abone olamasın
                    else Abone_ol_Btn.Enabled = true;
                    baglanti.Close();

                    SQLQuery3Parametreli("select COUNT(*) from Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @i and İzlenenenVideoID = @v and İzleyenID = @k", "@i", "@v", "@k", VideoSahibiID, VideonunID, izleyenkisiid);
                    if (rdr.Read())
                    {
                        if (rdr[0].ToString() == "0")
                        {
                            baglanti.Close();
                            SQLQuery3Parametreli("insert Görüntülenmeler (İzleyenID,İzlenenenkisiID,İzlenenenVideoID,İzlenenTarih)  values(@kisi,@izlenenkisi,@video,GETDATE())", "@kisi", "@izlenenkisi", "@video", izleyenkisiid, VideoSahibiID, VideonunID);
                            baglanti.Close();
                        }
                    }
                    baglanti.Close();
                }
                baglanti.Close();

                SQLQuery1Parametreli("select VideoPath,ID,VideoTarihi from Videolar Where VideoAdı=@videoadi", "@videoadi", list_Videolar.Text);

                if (rdr.Read())
                {
                    wmp.URL = rdr["VideoPath"].ToString();
                    Videonun_tarihi.Text = rdr["VideoTarihi"].ToString();
                }
                wmp.Ctlcontrols.play();
                baglanti.Close();
                YorumlarıGoster();

                GörüntülenmeGöster();
            }

        }
        private void Video_yükle_click(object sender, EventArgs e)
        {
            if (Video_Adi.TextLength <= 50)
            {
                string fileContent = string.Empty;
                string fileFullPath = string.Empty;
                string fileNme = string.Empty;
                string path = string.Empty;
                if (
                Fonksiyonlar.injectiondanKoru(Video_Adi.Text))
                {

                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "All Available Media Files (*.mp4;*.mkv)|*.mp4;*.mkv|mp4 (*.mp4)|*.mp4|mkv (*.mkv)|*.mkv";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.RestoreDirectory = true;
                        if ((Video_Adi.Text.Length) != 0)
                        {
                            //aynı video olmasının önüne geçmek

                            SQLQuery1Parametreli("select VideoAdı from Videolar where VideoAdı=@v", "@v", Video_Adi.Text);
                            if (rdr.Read())
                            {
                                MessageBox.Show("Sistemde aynı isimli video zaten var...");
                                baglanti.Close();
                            }
                            else
                            {

                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    fileFullPath = openFileDialog.FileName;
                                    fileNme = openFileDialog.SafeFileName;
                                    //pathin sonundan dosya adını çıkartmak
                                    path = fileFullPath.Replace(fileFullPath.Substring(fileFullPath.Length - fileNme.Length, fileNme.Length), "");
                                    // MessageBox.Show(fileNme+"\n"+fileFullPath+"\n"+path);

                                    string fileName = fileNme;
                                    string sourcePath = path;
                                    string targetPath = AppDomain.CurrentDomain.BaseDirectory + "Videolar";
                                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                                    // System.IO.File.Copy(sourceFile, destFile, true);
                                    FileSystem.CopyFile(fileFullPath, destFile, UIOption.AllDialogs);


                                    baglanti.Close();
                                    SQLQuery("insert Videolar (VideoAdı,VideoPath,SahibiID,PaylasildiDurum,VideoTarihi) values ('" + Video_Adi.Text + "', '" + destFile + "', '" + KullanıcıIDsiSession + "', '" + 0 + "'" + ",GETDATE() )");
                                    baglanti.Close();
                                    string a = null;
                                    SQLQuery1Parametreli("select ID from Videolar Where VideoAdı=@videoismi", "@videoismi", Video_Adi.Text);
                                    if (rdr.Read()) { a = rdr["ID"].ToString(); }
                                    baglanti.Close();

                                    string[] paramlar = new string[] { "@id", "@gr", "@bgn", "@bgme", "@Yrm" };
                                    string[] degerler = new string[] { a, "0", "0", "0", "0" };
                                    SQLQuery5Parametreli("insert Istatistik (VideoID,GoruntulenmeSayisi,BegenmeSayisi,BegenmemeSayisi,YorumSayisi) values (@id,@gr,@bgn,@bgme,@Yrm)", paramlar, degerler);
                                    baglanti.Close();

                                    //temizlemeden yazma
                                    SQLQuery("SELECT top 1 * FROM Videolar order by ID desc");
                                    while (rdr.Read()) list_Videolarim.Items.Add(rdr["VideoAdı"]);
                                    baglanti.Close();
                                }
                                else baglanti.Close();//video yükleme penceresinde iken vazgeçilirse ortaya çıkan hatanın önüne geçmek

                            }
                        }
                        else MessageBox.Show("Video İsmi girmeyi unuttunuz");

                    }

                }
                
            }
            else MessageBox.Show("Video İsmi uzunluğu 50'den fazla olamaz ");


        }

        //Grafiği temizleme
        public void ChartTemizle(System.Windows.Forms.DataVisualization.Charting.Chart temizlenecekChart)
        {
            foreach (var series in temizlenecekChart.Series)
            {
                series.Points.Clear();
            }
        }
        public void İstatistikChartDoldur(string yeri)
        {
            İstatistikChart.Series["Sayısı"].Points.Add(double.Parse(Fonksiyonlar.SayisiniGetir(" Select COUNT(*) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v", "@v", yeri)));
            İstatistikChart.Series["Sayısı"].Points.Add(double.Parse(Fonksiyonlar.SayisiniGetir(" Select COUNT(Begenme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v and Begenme = 'True' ", "@v", yeri)));
            İstatistikChart.Series["Sayısı"].Points.Add(double.Parse(Fonksiyonlar.SayisiniGetir(" Select COUNT(Begenmeme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v and Begenmeme = 'True' ", "@v", yeri)));
            İstatistikChart.Series["Sayısı"].Points.Add(double.Parse(Fonksiyonlar.SayisiniGetir(" Select COUNT(*) from Yorumlar inner join Videolar on Videolar.ID = Yorumlar.VideoID where VideoAdı = @v ", "@v", yeri)));
            İstatistikChart.Series["Sayısı"].Points[0].AxisLabel = "Görüntülenme";
            İstatistikChart.Series["Sayısı"].Points[1].AxisLabel = "Begenme";
            İstatistikChart.Series["Sayısı"].Points[2].AxisLabel = "Begenmeme";
            İstatistikChart.Series["Sayısı"].Points[3].AxisLabel = "Yorum";
            İstatistikChart.Series["Sayısı"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

        }
        private void IstatistikGoruntule(object sender, EventArgs e)
        {

            for (int i = 0; i == list_Videolarim.Items.Count; i++)
            {
                string[] paramlar = new string[] { "@GS", "@BS", "@BMS", "@YS", "@Vadi" };
                string[] degerler = new string[] { Fonksiyonlar.SayisiniGetir(" Select COUNT(*) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v", "@v", list_Videolarim.Items[i].ToString()),
                                                   Fonksiyonlar.SayisiniGetir(" Select COUNT(Begenme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v and Begenme = 'True' ", "@v", list_Videolarim.Items[i].ToString()),
                                                   Fonksiyonlar.SayisiniGetir(" Select COUNT(Begenmeme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v and Begenmeme = 'True' ", "@v", list_Videolarim.Items[i].ToString()),
                                                   Fonksiyonlar.SayisiniGetir(" Select COUNT(*) from Yorumlar inner join Videolar on Videolar.ID = Yorumlar.VideoID where VideoAdı = @v ", "@v", list_Videolarim.Items[i].ToString()),
                                                   list_Videolarim.Text };
                SQLQuery5Parametreli("Update Istatistik Set GoruntulenmeSayisi=@GS,BegenmeSayisi=@BS,BegenmemeSayisi=@BMS,YorumSayisi=@YS from Istatistik inner join Videolar on Videolar.ID = Istatistik.VideoID where VideoAdı =@Vadi", paramlar, degerler);
                baglanti.Close();
                SQLQuery("Select * from Istatistik inner join Videolar on Videolar.ID = Istatistik.VideoID where SahibiID ='" + VideoSahibiID + "'");
                baglanti.Close();
            }
            Fonksiyonlar.TümGridiDoldur(Kanal_Istatistik, "Select VideoAdı,PaylasildiDurum,VideoTarihi from Videolar inner join Oturum on Oturum.id=Videolar.SahibiID  Where Ad='" + lbl_Kullanici.Text + "'");
            Kanal_Istatistik.Columns[0].HeaderCell.Value = "Videonuzun Adı";
            Kanal_Istatistik.Columns[1].HeaderCell.Value = "Paylaşma Durumunuz";
            Kanal_Istatistik.Columns[2].HeaderCell.Value = "Paylaşılan Tarih";
            Kanal_Istatistik.Columns[0].Width = 100;
            Kanal_Istatistik.Columns[1].Width = 100;
            Kanal_Istatistik.Columns[2].Width = 100;
        }


        private void Kanal_Istatistik_SelectionChanged(object sender, EventArgs e)
        {
            ChartTemizle(İstatistikChart);
            İstatistikChartDoldur(Kanal_Istatistik.Rows[Kanal_Istatistik.CurrentCell.RowIndex].Cells[0].Value.ToString());
        }

        private void PaylasClck(object sender, EventArgs e)
        {

            if (list_Videolarim.SelectedIndex != -1)
            {
                SQLQuery1Parametreli("UPDATE Videolar SET PaylasildiDurum = 1 WHERE VideoAdı =@benimvideom", "@benimvideom", list_Videolarim.Text);
                baglanti.Close();
            }
            ListeleriYenile();

        }
        private void PaylasKapatClk(object sender, EventArgs e)
        {
            if (list_Videolarim.SelectedIndex != -1)
            {
                SQLQuery1Parametreli("UPDATE Videolar SET PaylasildiDurum = 0 WHERE VideoAdı =@benimvideom", "@benimvideom", list_Videolarim.Text);
                baglanti.Close();
            }
            ListeleriYenile();
        }

        int labelarasi = 0;

        private void YourumuYap(object sender, EventArgs e)
        {
            string a = "-1", b = null;
            SQLQuery1Parametreli("Select ID,id From Oturum INNER join Videolar on Videolar.SahibiID = Oturum.id where  VideoAdı=@videoismi", "@videoismi", list_Videolar.Text);
            if (rdr.Read())
            {
                b = (rdr["ID"].ToString());
            }
            baglanti.Close();
            SQLQuery1Parametreli("select id from Oturum WHERE Ad = @kullaniciismi", "@kullaniciismi", lbl_Kullanici.Text);
            if (rdr.Read())
            {
                a = rdr["id"].ToString();
            }
            baglanti.Close();
            if (
            Fonksiyonlar.injectiondanKoru(txt_Yorum.Text))
            {
                SQLQuery("Insert Yorumlar (VideoID,YorumSahipID,YorumIcerigi,YorumTarihi) values ('" + b + "', '" + a + "', '" + txt_Yorum.Text + "'" + ",GETDATE())");
                baglanti.Close();
            }

            YorumlarıGoster();
        }

        private void AboneOlClck(object sender, EventArgs e)
        {

            if (Abone_ol_Btn.Text == "Abone ol")
            {
                VideoİzleyeniBelirle();
                SQLQuery3Parametreli("insert Abonelikler (KimAboneID,KimeAboneID,OlunanVideoID,AboneOlmaTarihi) values (@k,@i,@v,GETDATE() )", "@k", "@i", "@v", izleyenkisiid, VideoSahibiID, VideonunID);
                //SQLQuery1Parametreli("UPDATE Istatistik set AboneSayisi=AboneSayisi+1 where VideoID=@videoİD" , "@videoİD", a );
                baglanti.Close();
                GörüntülenmeGöster();
            }

            else if (Abone_ol_Btn.Text == "Abone olundu")
            {
                VideoİzleyeniBelirle();
                SQLQuery2Parametreli("Delete Abonelikler from Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID inner join Oturum on Abonelikler.KimAboneID=Oturum.id where KimeAboneID = @i  and KimAboneID = @k", "@i", "@k", VideoSahibiID, izleyenkisiid);
                baglanti.Close();
                GörüntülenmeGöster();
            }
        }


        private void Begenmeme_clicked(object sender, EventArgs e)
        {
            VideoİzleyeniBelirle();
            SQLQuery2Parametreli("Select Begenmeme from Görüntülenmeler  inner join Videolar on Görüntülenmeler.İzlenenenVideoID=Videolar.ID inner join Oturum on Oturum.id=Görüntülenmeler.İzleyenID  WHERE VideoAdı =@v and Ad=@o ", "@v", "@o", list_Videolar.Text, lbl_Kullanici.Text);
            if (rdr.Read())
            {

                if (rdr[0].ToString() == "False" || rdr[0] == DBNull.Value)
                {
                    baglanti.Close();
                    //BegenmelerNullKaldır();
                    SQLQuery3Parametreli("UPDATE Görüntülenmeler  set Begenmeme='true' from  Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @i and İzlenenenVideoID = @v and İzleyenID = @k", "@i", "@v", "@k", VideoSahibiID, VideonunID, izleyenkisiid);
                    baglanti.Close();

                    GörüntülenmeGöster();
                }
                else
                {
                    baglanti.Close();
                    // BegenmelerNullKaldır();
                    SQLQuery3Parametreli("UPDATE Görüntülenmeler  set Begenmeme='False' from  Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @i and İzlenenenVideoID = @v and İzleyenID = @k", "@i", "@v", "@k", VideoSahibiID, VideonunID, izleyenkisiid);
                    baglanti.Close();

                    GörüntülenmeGöster();
                }
            }
            baglanti.Close();
        }
        private void begen_clcked(object sender, EventArgs e)
        {
            VideoİzleyeniBelirle();
            SQLQuery2Parametreli("Select Begenme from Görüntülenmeler  inner join Videolar on Görüntülenmeler.İzlenenenVideoID=Videolar.ID inner join Oturum on Oturum.id=Görüntülenmeler.İzleyenID  WHERE VideoAdı =@v and Ad=@o ", "@v", "@o", list_Videolar.Text, lbl_Kullanici.Text);
            if (rdr.Read())
            {
                if (rdr[0].ToString() == "False" || rdr[0] == DBNull.Value)
                {
                    baglanti.Close();
                    // BegenmelerNullKaldır();
                    SQLQuery3Parametreli("UPDATE Görüntülenmeler  set Begenme='true' from Görüntülenmeler  inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @i and İzlenenenVideoID = @v and İzleyenID = @k", "@i", "@v", "@k", VideoSahibiID, VideonunID, izleyenkisiid);
                    baglanti.Close();

                    GörüntülenmeGöster();
                }
                else
                {
                    baglanti.Close();
                    // BegenmelerNullKaldır();
                    SQLQuery3Parametreli("UPDATE Görüntülenmeler  set Begenme='False' from Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @i and İzlenenenVideoID = @v and İzleyenID = @k", "@i", "@v", "@k", VideoSahibiID, VideonunID, izleyenkisiid);
                    baglanti.Close();

                    GörüntülenmeGöster();
                }
            }
            baglanti.Close();
        }

        private void sifreGöster1_clcked(object sender, EventArgs e)
        {
            if (sifreGöster1.Checked == true)
            {
                txt_Sifre1.PasswordChar = '\0';

            }
            if (sifreGöster1.Checked == false)
            {
                txt_Sifre1.PasswordChar = '*';

            }
        }
        private void sifreGöster2_clcked(object sender, EventArgs e)
        {
            if (sifreGöster2.Checked == true)
            {
                txt_Sifre2.PasswordChar = '\0';

            }
            if (sifreGöster2.Checked == false)
            {
                txt_Sifre2.PasswordChar = '*';

            }
        }

        private void kllanıcı_degistir_Click(object sender, EventArgs e)
        {
            pnl_Giris.Visible = true;
            btn_AdmineGit.Visible = false;
            btn_AdmineGit.Enabled = false;
        }


        private void AnasayfaListeArama_TextChanged(object sender, EventArgs e)
        {
            list_Videolar.Items.Clear();

            komut = new SqlCommand("AnasayfaListeYenile", baglanti);
            baglanti.Open();
            komut.CommandType = CommandType.StoredProcedure;
            komut.Parameters.Add("VideoAdı", SqlDbType.NVarChar, 100).Value = "%" + AnasayfaListeAramaTextBox.Text + "%";
            rdr = komut.ExecuteReader();
            //Stored Procedure Kullanılarak yapılmıştır.
            while (rdr.Read())
            {
                list_Videolar.Items.Add(rdr["VideoAdı"]);

            }
            baglanti.Close();

        }


        private void ProfilListeAramaTextBox_TextChanged(object sender, EventArgs e)
        {
            list_Videolarim.Items.Clear();
            //Stored Procedure Kullanılarak yapılmıştır.
            komut = new SqlCommand("ProfilListeYenile", baglanti);
            baglanti.Open();
            komut.CommandType = CommandType.StoredProcedure;
            komut.Parameters.Add("VideoAdı", SqlDbType.NVarChar, 100).Value = "%" + ProfilListeAramaTextBox.Text + "%";
            rdr = komut.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr["Ad"].ToString() == lbl_Kullanici.Text)
                {
                    list_Videolarim.Items.Add(rdr["VideoAdı"]);

                }
            }
            baglanti.Close();
        }


        /// <fonksiyonlar>
        /// /////////
        /// <fonksiyonlar>
        /// 

        private void VideoİzleyeniBelirle()
        {
            SQLQuery1Parametreli("select id from Oturum  where Ad=@k", "@k", lbl_Kullanici.Text);
            if (rdr.Read())
            {
                izleyenkisiid = rdr["id"].ToString();

            }
            baglanti.Close();

            SQLQuery1Parametreli("select id,Videolar.ID,SahibiID,Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id where VideoAdı=@videoadi", "@videoadi", list_Videolar.Text);
            if (rdr.Read())
            {
                VideonunID = rdr["ID"].ToString();
                VideoSahibiID = rdr["SahibiID"].ToString();
            }
            baglanti.Close();
        }
        private void GörüntülenmeGöster()
        {
            //aynı kişinin aynı video izlemiş görünmesinin önüne geçer
            // SQLQuery("WITH cte AS (SELECT İzleyenID, İzlenenenkisiID, İzlenenenVideoID, ROW_NUMBER() OVER (PARTITION BY  İzleyenID, İzlenenenkisiID, İzlenenenVideoID ORDER BY İzleyenID, İzlenenenkisiID, İzlenenenVideoID) row_num FROM Görüntülenmeler)DELETE FROM cte WHERE row_num > 1;");
            //baglanti.Close();

            //görüntülenmeyi labele yazar
            SQLQuery1Parametreli(" Select COUNT(*) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v", "@v", list_Videolar.Text);
            if (rdr.Read()) lbl_GrntlmeSayisi.Text = rdr[0].ToString();
            baglanti.Close();

            //begenenler sayisilabele
            SQLQuery1Parametreli(" Select COUNT(Begenme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v and Begenme = 'True' ", "@v", list_Videolar.Text);
            if (rdr.Read()) lbl_Begen.Text = rdr[0].ToString();
            baglanti.Close();
            //begenmeyenlerin sayisi labele
            SQLQuery1Parametreli(" Select COUNT(Begenmeme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @v and Begenmeme = 'True' ", "@v", list_Videolar.Text);
            if (rdr.Read()) lbl_begenme.Text = rdr[0].ToString();
            baglanti.Close();
            VideoİzleyeniBelirle();
            //begenme buton göster
            SQLQuery3Parametreli("Select Begenme from  Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @i and İzlenenenVideoID = @v and İzleyenID = @k", "@i", "@v", "@k", VideoSahibiID, VideonunID, izleyenkisiid);
            if (rdr.Read())
            {
                if (rdr[0].ToString() == "True")
                {
                    btn_Begen.Text = "Beğenildi";
                    btn_begenme.Enabled = false;
                }
                else
                {
                    btn_Begen.Text = "Beğen";
                    btn_begenme.Enabled = true;
                }
            }
            //begenmeme buton göster
            baglanti.Close();
            SQLQuery3Parametreli("Select Begenmeme from  Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @i and İzlenenenVideoID = @v and İzleyenID = @k", "@i", "@v", "@k", VideoSahibiID, VideonunID, izleyenkisiid);
            if (rdr.Read())
            {
                if (rdr[0].ToString() == "True")
                {
                    btn_begenme.Text = "Beğenilmedi";
                    btn_Begen.Enabled = false;
                }
                else
                {
                    btn_begenme.Text = "Beğenme";
                    btn_Begen.Enabled = true;
                }
            }
            baglanti.Close();
            //kanal abone sayısını güncelleme
            SQLQuery1Parametreli("select Count(*) from Abonelikler inner join Videolar on Videolar.ID=Abonelikler.OlunanVideoID where SahibiID = @v", "@v", VideoSahibiID);
            if (rdr.Read()) Abone_sayisi.Text = rdr[0].ToString();
            baglanti.Close();

            //Videoya değil kanala abone olma butonunu 
            SQLQuery2Parametreli("Select COUNT(*) Abonelikler from Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID inner join Oturum on Abonelikler.KimAboneID=Oturum.id where KimeAboneID = @kim  and KimAboneID =@kime", "@kim", "@kime", VideoSahibiID, izleyenkisiid);
            if (rdr.Read())
            {
                if (rdr[0].ToString() == "0") { Abone_ol_Btn.Text = "Abone ol"; }
                else { Abone_ol_Btn.Text = "Abone olundu"; }
            }
            baglanti.Close();
        }



        private void YorumlarıGoster()
        {
            pnl_Yorum.Controls.Clear();
            labelarasi = 0;
            SQLQuery1Parametreli("Select VideoID,Videolar.ID,AD,YorumIcerigi,YorumTarihi from Yorumlar inner join Videolar on Videolar.ID=Yorumlar.VideoID inner join Oturum on Oturum.id=Yorumlar.YorumSahipID  where VideoAdı =@videonunismi", "@videonunismi", list_Videolar.Text);
            while (rdr.Read())
            {
                if (rdr["VideoID"].ToString() == rdr["ID"].ToString())
                {
                    Label label = new Label();
                    label.Height = label.Height + 20;
                    label.AutoSize = true;
                    label.Text = rdr["Ad"].ToString() + "                                              " + rdr["YorumTarihi"].ToString() + "\n           " + rdr["YorumIcerigi"].ToString();
                    label.Top += labelarasi;
                    labelarasi += 50;
                    pnl_Yorum.Controls.Add(label);
                }
            }
            baglanti.Close();
        }

        private void SQLQuery1Parametreli(string s, string parametre, string degeri)
        {
            komut = new SqlCommand();
            komut.Parameters.AddWithValue(parametre, degeri);
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = s;
            rdr = komut.ExecuteReader();

        }

        private void SQLQuery2Parametreli(string s, string parametre1, string parametre2, string degeri1, string degeri2)
        {
            komut = new SqlCommand();
            komut.Parameters.AddWithValue(parametre1, degeri1);
            komut.Parameters.AddWithValue(parametre2, degeri2);
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = s;
            rdr = komut.ExecuteReader();

        }

        private void SQLQuery3Parametreli(string s, string parametre1, string parametre2, string parametre3, string degeri1, string degeri2, string degeri3)
        {
            komut = new SqlCommand();
            komut.Parameters.AddWithValue(parametre1, degeri1);
            komut.Parameters.AddWithValue(parametre2, degeri2);
            komut.Parameters.AddWithValue(parametre3, degeri3);
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = s;
            rdr = komut.ExecuteReader();

        }
        public void SQLQuery(string s)
        {
            komut = new SqlCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = s;
            rdr = komut.ExecuteReader();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SQLQuery5Parametreli(string s, string[] parametreler, string[] degerler)
        {
            komut = new SqlCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = s;
            komut.Parameters.AddWithValue(parametreler[0], degerler[0]);
            komut.Parameters.AddWithValue(parametreler[1], degerler[1]);
            komut.Parameters.AddWithValue(parametreler[2], degerler[2]);
            komut.Parameters.AddWithValue(parametreler[3], degerler[3]);
            komut.Parameters.AddWithValue(parametreler[4], degerler[4]);
            rdr = komut.ExecuteReader();
        }



        private void ListeleriYenile()
        {
            list_Videolar.Items.Clear();
            list_Videolarim.Items.Clear();
            SQLQuery("Select * From Videolar where PaylasildiDurum=1");
            while (rdr.Read())
            {
                list_Videolar.Items.Add(rdr["VideoAdı"]);

            }
            baglanti.Close();

            SQLQuery("Select * From Oturum INNER join Videolar on Videolar.SahibiID=Oturum.id");
            while (rdr.Read())
            {
                if (rdr["Ad"].ToString() == lbl_Kullanici.Text)
                {
                    list_Videolarim.Items.Add(rdr["VideoAdı"]);

                }
            }
            baglanti.Close();
        }


    }
}
