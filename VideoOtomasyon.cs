using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Npgsql;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;

namespace VideoOtomasyon
{
    public partial class VideoOtomasyon : Form
    {
        //OguzDTO.mdf ve OguzDTO.ldf isimli olan dosyaları SQL servere attach yapmadan önce güvenlik ilkelerinden bu dosyalar için  bütün erişim izinlerinin alınması gerekmektedir.
        //Program derlenmeden önce VideoOtomasyon.cs , Fonksiyonlar.cs , AdminPaneli.cs , ŞifreDegistirme.cs dosyalarındaki "connectionString"-lerinin güncellenmesi gerekmektedir.
        string connectionString = "Server=localhost;Port=5432;Database=VideoOtomasyon;User Id=postgres;Password=123;";

        public static string VideonunID = null, VideoSahibiID = null, izleyenkisiid = null, KullanıcınınAdı = null;
 
        public VideoOtomasyon()
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Fonksiyonlar.con.ConnectionString = connectionString;
            

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
           // VideoİzleyeniBelirle();

            Fonksiyonlar.DataGridRowGüncelle(aboneolgmkanal, "select SahibiID,AboneOlmaTarihi,VideoAdı,VideoTarihi from Abonelikler  Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID   where KimAboneID= "+ izleyenkisiid,"Select Oturum.Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id inner join Abonelikler on Abonelikler.KimAboneID=Oturum.id where KimAboneID=@k", "@k");
            aboneolgmkanal.Columns[0].HeaderText = "Abone Olduğunuz Kanalın İsmi";
            aboneolgmkanal.Columns[1].HeaderText = "Abone Olduğunuz Tarih";
            aboneolgmkanal.Columns[2].HeaderText = "Abone Olduğunuz Videosu";
            aboneolgmkanal.Columns[3].HeaderText = "Videonun Paylaşılma Tarihi";
            //Fonksiyonlar.DataGridRowGüncelle(aboneolankanal, "select KimAboneID,AboneOlmaTarihi,VideoAdı,VideoTarihi from Abonelikler Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID   where KimeAboneID=" + izleyenkisiid, "Select Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id inner join Abonelikler on Abonelikler.KimAboneID=Oturum.id where KimAboneID=@k", "@k");
            Fonksiyonlar.TümGridiDoldur(aboneolankanal, "select Oturum.ad,AboneOlmaTarihi,VideoAdı,VideoTarihi from Abonelikler Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID  inner join Oturum on Abonelikler.KimAboneID=Oturum.ID where KimeAboneID=" + izleyenkisiid);
         
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

                            Fonksiyonlar.paramatrelidata("select id,Ad from Oturum WHERE Ad = @0", new[] { txt_Kullanici1.Text });

                            if (Fonksiyonlar.rdr.Read())//sqli okursa kullanıcı ve şifresi doğrudur
                            {
                                MessageBox.Show("Bu kullanıcı adı zaten var...");
                            }
                            else
                            {
                                string girissifresi = Fonksiyonlar.md5ilesifrele(txt_Sifre1.Text);
                                Fonksiyonlar.con.Close();
                                SQLQuery("insert into Oturum (Ad,Sifre,AdminYetkisi,SonGiris) values ('" + txt_Kullanici1.Text + "','" + girissifresi + "','" + "False" + "'" + ",NOW() )");
                                Fonksiyonlar.con.Close();
                                MessageBox.Show("Kullanıcı Başarıyla Kaydedildi.");
                            }
                            Fonksiyonlar.con.Close();
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
                    if (Fonksiyonlar.rdr.Read())//sqli okursa kullanıcı ve şifresi doğrudur
                    {
                        izleyenkisiid = Fonksiyonlar.rdr["id"].ToString();
                        KullanıcınınAdı = Fonksiyonlar.rdr["Ad"].ToString();
                        MessageBox.Show("Giriş Başarılı");
                        lbl_Kullanici.Text = Fonksiyonlar.rdr["Ad"].ToString();

                        Fonksiyonlar.con.Close();
                        Fonksiyonlar.paramatrelidata("UPDATE Oturum SET SonGiris=NOW() WHERE Ad=@0", new[] { txt_Kullanici2.Text });

                        pnl_Giris.Visible = false;

                        if (Fonksiyonlar.YetkiKontrol()) { btn_AdmineGit.Visible = true; btn_AdmineGit.Enabled = true; }

                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifresi yanlış");

                    }
                    Fonksiyonlar.con.Close();



                    Fonksiyonlar.con.Close();
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
                    Fonksiyonlar.paramatrelidata("select Videolar.ID,SahibiID from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id where VideoAdı=@0", new[] { list_Videolar.Text });
                    if (Fonksiyonlar.rdr.Read())
                    {
                        VideonunID = Fonksiyonlar.rdr["ID"].ToString();
                        VideoSahibiID = Fonksiyonlar.rdr["SahibiID"].ToString();
                    }
                Fonksiyonlar.con.Close();

                
                Fonksiyonlar.paramatrelidata("select Oturum.id,Videolar.ID,SahibiID,Ad from Oturum inner join Videolar on Videolar.SahibiID=Oturum.id where VideoAdı=@0", new[] { list_Videolar.Text });
                if (Fonksiyonlar.rdr.Read())
                {
                    //Hangi kanalın videosunun izlendiğinin gösterilmesi
                    lbl_izlenenKanal.Text = Fonksiyonlar.rdr["Ad"].ToString(); //izlenen kanalın adını yazmak
                    if (Fonksiyonlar.rdr["Ad"].ToString() == lbl_Kullanici.Text) Abone_ol_Btn.Enabled = false;  //kişi kendisine abone olamasın
                    else Abone_ol_Btn.Enabled = true;
                    Fonksiyonlar.con.Close();
                    

                    //kişi aynı videoyu izlememişse veri tabanına kaydedilsin kısmı
                    Fonksiyonlar.paramatrelidata("select COUNT(*) from Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @0 and İzlenenenVideoID = @1 and İzleyenID = @2", new[] { VideoSahibiID, VideonunID, izleyenkisiid });
                    if (Fonksiyonlar.rdr.Read())
                    {
                        if (Fonksiyonlar.rdr[0].ToString() == "0")
                        {
                            Fonksiyonlar.con.Close();
                            Fonksiyonlar.paramatrelidata("insert into Görüntülenmeler (İzleyenID,İzlenenenkisiID,İzlenenenVideoID,İzlenenTarih)  values (@0,@1,@2,NOW())", new[] { izleyenkisiid, VideoSahibiID, VideonunID });
                            Fonksiyonlar.con.Close();
                        }
                    }
                    Fonksiyonlar.con.Close();
                }
                Fonksiyonlar.con.Close();

                Fonksiyonlar.paramatrelidata("select VideoPath,ID,VideoTarihi from Videolar Where VideoAdı=@0", new[] { list_Videolar.Text });

                if (Fonksiyonlar.rdr.Read())
                {
                    wmp.URL = Fonksiyonlar.rdr["VideoPath"].ToString();
                    Videonun_tarihi.Text = Fonksiyonlar.rdr["VideoTarihi"].ToString();
                }
                wmp.Ctlcontrols.play();
                Fonksiyonlar.con.Close();
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

                            Fonksiyonlar.paramatrelidata("select VideoAdı from Videolar where VideoAdı=@0", new[] { Video_Adi.Text });
                            if (Fonksiyonlar.rdr.Read())
                            {
                                MessageBox.Show("Sistemde aynı isimli video zaten var...");
                                Fonksiyonlar.con.Close();
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


                                    Fonksiyonlar.con.Close();
                                    SQLQuery("insert into Videolar (VideoAdı,VideoPath,SahibiID,PaylasildiDurum,VideoTarihi) values ('" + Video_Adi.Text + "', '" + destFile + "', '" + izleyenkisiid + "', '" + "FALSE" + "'" + ",NOW() )");
                                    Fonksiyonlar.con.Close();
                                    string a = null;
                                    Fonksiyonlar.paramatrelidata("select ID from Videolar Where VideoAdı=@0", new[] { Video_Adi.Text });
                                    if (Fonksiyonlar.rdr.Read()) { a = Fonksiyonlar.rdr["ID"].ToString(); }
                                    Fonksiyonlar.con.Close();

                                    string[] paramlar = new string[] { "@id", "@gr", "@bgn", "@bgme", "@Yrm" };
                                    string[] degerler = new string[] { a, "0", "0", "0", "0" };
                                    Fonksiyonlar.paramatrelidata("insert into Istatistik (VideoID,GoruntulenmeSayisi,BegenmeSayisi,BegenmemeSayisi,YorumSayisi) values (@0,@1,@2,@3,@4)", degerler);
                                    Fonksiyonlar.con.Close();

                                    //temizlemeden yazma
                                    SQLQuery("SELECT * FROM Videolar order by ID desc fetch first 1 rows only");
                                    while (Fonksiyonlar.rdr.Read()) list_Videolarim.Items.Add(Fonksiyonlar.rdr["VideoAdı"]);
                                    Fonksiyonlar.con.Close();
                                }
                                else Fonksiyonlar.con.Close();//video yükleme penceresinde iken vazgeçilirse ortaya çıkan hatanın önüne geçmek

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
                Fonksiyonlar.paramatrelidata("Update Istatistik Set GoruntulenmeSayisi=@0,BegenmeSayisi=@1,BegenmemeSayisi=@2,YorumSayisi=@3 from Istatistik inner join Videolar on Videolar.ID = Istatistik.VideoID where VideoAdı =@4" ,degerler);
                Fonksiyonlar.con.Close();
                SQLQuery("Select * from Istatistik inner join Videolar on Videolar.ID = Istatistik.VideoID where SahibiID =" + VideoSahibiID);
                Fonksiyonlar.con.Close();
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
                Fonksiyonlar.paramatrelidata("UPDATE Videolar SET PaylasildiDurum = True WHERE VideoAdı =@0", new[] { list_Videolarim.Text });
                Fonksiyonlar.con.Close();
            }
            ListeleriYenile();

        }
        private void PaylasKapatClk(object sender, EventArgs e)
        {
            if (list_Videolarim.SelectedIndex != -1)
            {
                Fonksiyonlar.paramatrelidata("UPDATE Videolar SET PaylasildiDurum = False WHERE VideoAdı =@0", new[] { list_Videolarim.Text });
                Fonksiyonlar.con.Close();
            }
            ListeleriYenile();
        }

        int labelarasi = 0;

        private void YourumuYap(object sender, EventArgs e)
        {
            string a = "-1", b = null;
            Fonksiyonlar.paramatrelidata("Select Videolar.ID,Oturum.id From Oturum INNER join Videolar on Videolar.SahibiID = Oturum.id where  VideoAdı=@0", new[] { list_Videolar.Text });
            if (Fonksiyonlar.rdr.Read())
            {
                b = (Fonksiyonlar.rdr["ID"].ToString());
            }
            Fonksiyonlar.con.Close();
            
            if (
            Fonksiyonlar.injectiondanKoru(txt_Yorum.Text))
            {
                SQLQuery("Insert into Yorumlar (VideoID,YorumSahipID,YorumIcerigi,YorumTarihi) values ('" + b + "', '" + izleyenkisiid + "', '" + txt_Yorum.Text + "'" + ",NOW())");
                Fonksiyonlar.con.Close();
            }

            YorumlarıGoster();
        }

        private void AboneOlClck(object sender, EventArgs e)
        {

            if (Abone_ol_Btn.Text == "Abone ol")
            {
               
                Fonksiyonlar.paramatrelidata("insert into Abonelikler (KimAboneID,KimeAboneID,OlunanVideoID,AboneOlmaTarihi) values (@0,@1,@2,NOW() )", new[] { izleyenkisiid, VideoSahibiID, VideonunID });
                //SQLQuery1Parametreli("UPDATE Istatistik set AboneSayisi=AboneSayisi+1 where VideoID=@videoİD" , "@videoİD", a );
                Fonksiyonlar.con.Close();
                GörüntülenmeGöster();
            }

            else if (Abone_ol_Btn.Text == "Abone olundu")
            {
        
                Fonksiyonlar.paramatrelidata("Delete from Abonelikler using Oturum , Videolar where Abonelikler.OlunanVideoID=Videolar.ID and Abonelikler.KimAboneID=Oturum.id and KimeAboneID = @0  and KimAboneID = @1", new[] { VideoSahibiID, izleyenkisiid });
                Fonksiyonlar.con.Close();
                GörüntülenmeGöster();
            }
        }


        private void Begenmeme_clicked(object sender, EventArgs e)
        {

            Fonksiyonlar.paramatrelidata("Select Begenmeme from Görüntülenmeler  inner join Videolar on Görüntülenmeler.İzlenenenVideoID=Videolar.ID inner join Oturum on Oturum.id=Görüntülenmeler.İzleyenID  WHERE VideoAdı =@0 and Ad=@1 ", new[] { list_Videolar.Text, lbl_Kullanici.Text });
            if (Fonksiyonlar.rdr.Read())
            {

                if (Fonksiyonlar.rdr[0].ToString() == "False" || Fonksiyonlar.rdr[0] == DBNull.Value)
                {
                    Fonksiyonlar.con.Close();
                    //BegenmelerNullKaldır();
                    Fonksiyonlar.paramatrelidata("UPDATE Görüntülenmeler  set Begenmeme=true from  Videolar,Oturum where Görüntülenmeler.İzlenenenVideoID = Videolar.ID and Oturum.id = Görüntülenmeler.İzleyenID and İzlenenenkisiID = @0 and İzlenenenVideoID = @1 and İzleyenID = @2", new[] { VideoSahibiID, VideonunID, izleyenkisiid });
                    Fonksiyonlar.con.Close();

                    GörüntülenmeGöster();
                }
                else
                {
                    Fonksiyonlar.con.Close();
                    // BegenmelerNullKaldır();
                    Fonksiyonlar.paramatrelidata("UPDATE Görüntülenmeler  set Begenmeme=False from  Videolar,Oturum where Görüntülenmeler.İzlenenenVideoID = Videolar.ID and Oturum.id = Görüntülenmeler.İzleyenID and İzlenenenkisiID = @0 and İzlenenenVideoID = @1 and İzleyenID = @2", new[] { VideoSahibiID, VideonunID, izleyenkisiid });
                    Fonksiyonlar.con.Close();

                    GörüntülenmeGöster();
                }
            }
            Fonksiyonlar.con.Close();
        }
        private void begen_clcked(object sender, EventArgs e)
        {
            Fonksiyonlar.paramatrelidata("Select Begenme from Görüntülenmeler  inner join Videolar on Görüntülenmeler.İzlenenenVideoID=Videolar.ID inner join Oturum on Oturum.id=Görüntülenmeler.İzleyenID  WHERE VideoAdı =@0 and Ad=@1 ", new[] { list_Videolar.Text, lbl_Kullanici.Text });
            if (Fonksiyonlar.rdr.Read())
            {
                if (Fonksiyonlar.rdr[0].ToString() == "False" || Fonksiyonlar.rdr[0] == DBNull.Value)
                {
                    Fonksiyonlar.con.Close();
                    // BegenmelerNullKaldır();
                    Fonksiyonlar.paramatrelidata("UPDATE Görüntülenmeler  set Begenme=true from Oturum,Videolar  where Görüntülenmeler.İzlenenenVideoID = Videolar.ID and Oturum.id = Görüntülenmeler.İzleyenID  and İzlenenenkisiID = @0 and İzlenenenVideoID = @1 and İzleyenID = @2", new[] { VideoSahibiID, VideonunID, izleyenkisiid });
                    Fonksiyonlar.con.Close();

                    GörüntülenmeGöster();
                }
                else
                {
                    Fonksiyonlar.con.Close();
                    // BegenmelerNullKaldır();
                    Fonksiyonlar.paramatrelidata("UPDATE Görüntülenmeler  set Begenme=False from Oturum,Videolar where  Görüntülenmeler.İzlenenenVideoID = Videolar.ID and Oturum.id = Görüntülenmeler.İzleyenID and İzlenenenkisiID = @0 and İzlenenenVideoID = @1 and İzleyenID = @2", new[] { VideoSahibiID, VideonunID, izleyenkisiid });
                    Fonksiyonlar.con.Close();

                    GörüntülenmeGöster();
                }
            }
            Fonksiyonlar.con.Close();
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

            Fonksiyonlar.cmd = new NpgsqlCommand("SELECT  AnasayfaListeYenile(@VAd)", Fonksiyonlar.con);
            Fonksiyonlar.con.Open();

            Fonksiyonlar.cmd.Parameters.Add("@VAd", NpgsqlTypes.NpgsqlDbType.Text, 100).Value = "%" + AnasayfaListeAramaTextBox.Text + "%";
            Fonksiyonlar.rdr = Fonksiyonlar.cmd.ExecuteReader();
            //Function Kullanılarak yapılmıştır.
            while (Fonksiyonlar.rdr.Read())
            {
                list_Videolar.Items.Add(Fonksiyonlar.rdr["AnasayfaListeYenile"]);

            }
            Fonksiyonlar.con.Close();
        
        }


        private void ProfilListeAramaTextBox_TextChanged(object sender, EventArgs e)
        {
            
            list_Videolarim.Items.Clear();
            //Function Kullanılarak yapılmıştır.
            Fonksiyonlar.cmd = new NpgsqlCommand("Select * from profillisteyenile(@viad)", Fonksiyonlar.con);
            Fonksiyonlar.cmd.Parameters.Add("@viad", NpgsqlTypes.NpgsqlDbType.Text).NpgsqlValue = "%"+ProfilListeAramaTextBox.Text+"%";
            Fonksiyonlar.cmd.CommandType = CommandType.Text;
            Fonksiyonlar.con.Open();
            Fonksiyonlar.rdr = Fonksiyonlar.cmd.ExecuteReader();
            
                while (Fonksiyonlar.rdr.Read())
                {
                if (Fonksiyonlar.rdr["Ad"].ToString() == lbl_Kullanici.Text)
                
                    list_Videolarim.Items.Add(Fonksiyonlar.rdr["VideoAdı"]);
                }
            Fonksiyonlar.con.Close();

            
        }



        /// <fonksiyonlar>
        /// /////////
        /// <fonksiyonlar>
        /// 

      
        private void GörüntülenmeGöster()
        {
            //aynı kişinin aynı video izlemiş görünmesinin önüne geçer
            // SQLQuery("WITH cte AS (SELECT İzleyenID, İzlenenenkisiID, İzlenenenVideoID, ROW_NUMBER() OVER (PARTITION BY  İzleyenID, İzlenenenkisiID, İzlenenenVideoID ORDER BY İzleyenID, İzlenenenkisiID, İzlenenenVideoID) row_num FROM Görüntülenmeler)DELETE FROM cte WHERE row_num > 1;");
            //baglanti.Close();

            //görüntülenmeyi labele yazar
            Fonksiyonlar.paramatrelidata(" Select COUNT(*) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @0",new[] { list_Videolar.Text });
            if (Fonksiyonlar.rdr.Read()) { lbl_GrntlmeSayisi.Text = Fonksiyonlar.rdr[0].ToString();  }
            Fonksiyonlar.con.Close();

            //begenenler sayisilabele
            Fonksiyonlar.paramatrelidata(" Select COUNT(Begenme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @0 and Begenme = True ", new[] { list_Videolar.Text });
            if (Fonksiyonlar.rdr.Read()) lbl_Begen.Text = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();
            //begenmeyenlerin sayisi labele
            Fonksiyonlar.paramatrelidata(" Select COUNT(Begenmeme) from Görüntülenmeler inner join Videolar on Videolar.ID = Görüntülenmeler.İzlenenenVideoID where VideoAdı = @0 and Begenmeme = True ", new[] { list_Videolar.Text });
            if (Fonksiyonlar.rdr.Read()) lbl_begenme.Text = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();

            //begenme buton göster
            Fonksiyonlar.paramatrelidata("Select Begenme from  Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @0 and İzlenenenVideoID = @1 and İzleyenID = @2", new[] { VideoSahibiID, VideonunID, izleyenkisiid });
            if (Fonksiyonlar.rdr.Read())
            {
                
                if (Fonksiyonlar.rdr[0].ToString() == "True")
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
            Fonksiyonlar.con.Close();

            Fonksiyonlar.paramatrelidata("Select Begenmeme from  Görüntülenmeler inner join Videolar on Görüntülenmeler.İzlenenenVideoID = Videolar.ID inner join Oturum on Oturum.id = Görüntülenmeler.İzleyenID where İzlenenenkisiID = @0 and İzlenenenVideoID = @1 and İzleyenID = @2", new[] { VideoSahibiID, VideonunID, izleyenkisiid });
            if (Fonksiyonlar.rdr.Read())
            {
            
                if (Fonksiyonlar.rdr[0].ToString() == "True")
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
            Fonksiyonlar.con.Close();
            //kanal abone sayısını güncelleme
            Fonksiyonlar.paramatrelidata("select Count(*) from Abonelikler inner join Videolar on Videolar.ID=Abonelikler.OlunanVideoID where SahibiID = @0", new[] { VideoSahibiID });
            if (Fonksiyonlar.rdr.Read()) Abone_sayisi.Text = Fonksiyonlar.rdr[0].ToString();
            Fonksiyonlar.con.Close();

            //Videoya değil kanala abone olma butonunu 
            Fonksiyonlar.paramatrelidata("Select COUNT(*) from Abonelikler inner join Videolar on Abonelikler.OlunanVideoID=Videolar.ID inner join Oturum on Abonelikler.KimAboneID=Oturum.id where KimeAboneID = @0  and KimAboneID =@1", new[] { VideoSahibiID, izleyenkisiid });
            if (Fonksiyonlar.rdr.Read())
            {
                if (Fonksiyonlar.rdr[0].ToString() == "0") { Abone_ol_Btn.Text = "Abone ol"; }
                else { Abone_ol_Btn.Text = "Abone olundu"; }
            }
            Fonksiyonlar.con.Close();
        }



        private void YorumlarıGoster()
        {
            pnl_Yorum.Controls.Clear();
            labelarasi = 0;
            Fonksiyonlar.paramatrelidata("Select VideoID,Videolar.ID,AD,YorumIcerigi,YorumTarihi from Yorumlar inner join Videolar on Videolar.ID=Yorumlar.VideoID inner join Oturum on Oturum.id=Yorumlar.YorumSahipID  where VideoAdı =@0", new[] { list_Videolar.Text });
            while (Fonksiyonlar.rdr.Read())
            {
                if (Fonksiyonlar.rdr["VideoID"].ToString() == Fonksiyonlar.rdr["ID"].ToString())
                {
                    Label label = new Label();
                    label.Height = label.Height + 20;
                    label.AutoSize = true;
                    label.Text = Fonksiyonlar.rdr["Ad"].ToString() + "                                              " + Fonksiyonlar.rdr["YorumTarihi"].ToString() + "\n           " + Fonksiyonlar.rdr["YorumIcerigi"].ToString();
                    label.Top += labelarasi;
                    labelarasi += 50;
                    pnl_Yorum.Controls.Add(label);
                }
            }
            Fonksiyonlar.con.Close();
        }

        

        private void pnl_Giris_Paint(object sender, PaintEventArgs e)
        {

        }

        public void SQLQuery(string s)
        {
            Fonksiyonlar.cmd = new NpgsqlCommand();
            Fonksiyonlar.con.Open();
            Fonksiyonlar.cmd.Connection = Fonksiyonlar.con;
            Fonksiyonlar.cmd.CommandText = s;
            Fonksiyonlar.rdr = Fonksiyonlar.cmd.ExecuteReader();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       



        private void ListeleriYenile()
        {
            list_Videolar.Items.Clear();
            list_Videolarim.Items.Clear();
            SQLQuery("Select * From Videolar where PaylasildiDurum=True");
            while (Fonksiyonlar.rdr.Read())
            {
                list_Videolar.Items.Add(Fonksiyonlar.rdr["VideoAdı"]);

            }
            Fonksiyonlar.con.Close();

            SQLQuery("Select * From Oturum INNER join Videolar on Videolar.SahibiID=Oturum.id");
            while (Fonksiyonlar.rdr.Read())
            {
                if (Fonksiyonlar.rdr["Ad"].ToString() == lbl_Kullanici.Text)
                {
                    list_Videolarim.Items.Add(Fonksiyonlar.rdr["VideoAdı"]);

                }
            }
            Fonksiyonlar.con.Close();
        }


    }
}
