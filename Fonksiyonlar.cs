using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Npgsql;

namespace VideoOtomasyon
{
    internal class Fonksiyonlar
    {
        static public string connectionString = "Server=localhost;Port=5432;Database=VideoOtomasyon;User Id=postgres;Password=123;";
        static public NpgsqlConnection con = new NpgsqlConnection(connectionString);
        static public NpgsqlCommand cmd = new NpgsqlCommand();
        static public NpgsqlDataAdapter da;
        static public DataSet ds;
        static public NpgsqlDataReader rdr;
        

        static public DataGridView TümGridiDoldur(DataGridView grit, string Sorgunuz)
        {

            con = new NpgsqlConnection(connectionString);
            da = new NpgsqlDataAdapter(Sorgunuz, con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, Sorgunuz);
            grit.DataSource = ds.Tables[Sorgunuz];
            con.Close();

            return grit;
        }

        static public DataTable DatasetimiziDoldur( string Sorgunuz)
        {

            con = new NpgsqlConnection(connectionString);
            da = new NpgsqlDataAdapter(Sorgunuz, con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, Sorgunuz);       
            con.Close();
            return ds.Tables[0];
        }
        static public bool SifrelemeKuralları(string s)
        {
            var uygunsifre = Regex.Match(s, @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,})");
            if (uygunsifre.Success)
                return true;
            else
            {
                MessageBox.Show("Şifreniz en az 6 uzunlukta olmalı ve en az 1 kücük harf,en az bir büyük harf ve rakam içermelidir.");
                return false;
            }

        }

        //yollanan gridin ilk sorgu ile doldurulması ve ikinci bir sorgudaki data ile sadece ilk sütünun silinip tekarar doldurulması
        static public DataGridView DataGridRowGüncelle(DataGridView grit, string Sorgu, string güncellenecekSorgu, string Gparam)
        {
            grit.Columns.Clear();
            con = new NpgsqlConnection(connectionString);
            da = new NpgsqlDataAdapter(Sorgu, con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, Sorgu);
            grit.DataSource = ds.Tables[Sorgu];
            con.Close();
            string[] a = new string[grit.Rows.Count];
            for (int i = 0; i < grit.Rows.Count - 1; i++)
            {
                a[i] = grit.Rows[i].Cells[0].Value.ToString();
              
                cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = güncellenecekSorgu;
                cmd.Parameters.AddWithValue(Gparam, Convert.ToInt32(a[i]));
                con.Open();
                
                rdr = cmd.ExecuteReader();
                if (rdr.Read()) { a[i] = rdr[0].ToString(); }
                con.Close();
            }
            grit.Columns.RemoveAt(0);
            grit.Columns.Insert(0, new DataGridViewTextBoxColumn());
            for (int i = 0; i < grit.Rows.Count - 1; i++)
            {
                grit.Rows[i].Cells[0].Value = a[i];
            }

            return grit;
        }
        //yollanan ilk sorgu ile doldurulan bir grid ve oluşturulan gridin başka bir sorgunun sonucundaki veriler ile istenilen sütünunun doldurulması
        static public DataGridView DataGridRowGüncelleVeSeç(DataGridView grit, string Sorgu, string güncellenecekSorgu, string Gparam, int güncellenecekSütün)
        {
            grit.Columns.Clear();
            con = new NpgsqlConnection(connectionString);
            da = new NpgsqlDataAdapter(Sorgu, con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, Sorgu);
            grit.DataSource = ds.Tables[Sorgu];
            con.Close();
            string[] a = new string[grit.Rows.Count];
            for (int i = 0; i < grit.Rows.Count - 1; i++)
            {
                a[i] = grit.Rows[i].Cells[güncellenecekSütün].Value.ToString();

                cmd = new NpgsqlCommand();
                cmd.Parameters.AddWithValue(Gparam, Convert.ToInt32(a[i]));
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = güncellenecekSorgu;
                rdr = cmd.ExecuteReader();
                if (rdr.Read()) { a[i] = rdr[0].ToString(); }
                con.Close();
            }
            grit.Columns.RemoveAt(güncellenecekSütün);
            grit.Columns.Insert(güncellenecekSütün, new DataGridViewTextBoxColumn());
            for (int i = 0; i < grit.Rows.Count - 1; i++)
            {
                grit.Rows[i].Cells[güncellenecekSütün].Value = a[i];
            }

            return grit;
        }

        //Hali Hazırda bir gridin yollanıp bir sorgu ile belirlenenen sütünün değiştirilmesi
        static public DataGridView DataGridRowDeğiştir(DataGridView grit, string güncellenecekSorgu, string Gparam, int güncellenecekSütün)
        {
            string[] a = new string[grit.Rows.Count];
            for (int i = 0; i < grit.Rows.Count - 1; i++)
            {
                a[i] = grit.Rows[i].Cells[güncellenecekSütün].Value.ToString();

                cmd = new NpgsqlCommand();
                cmd.Parameters.AddWithValue(Gparam, Convert.ToInt32(a[i]));
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = güncellenecekSorgu;
                rdr = cmd.ExecuteReader();
                if (rdr.Read()) { a[i] = rdr[0].ToString(); }
                con.Close();
            }
            grit.Columns.RemoveAt(güncellenecekSütün);
            grit.Columns.Insert(güncellenecekSütün, new DataGridViewTextBoxColumn());
            for (int i = 0; i < grit.Rows.Count - 1; i++)
            {
                grit.Rows[i].Cells[güncellenecekSütün].Value = a[i];
            }

            return grit;
        }

        public static bool injectiondanKoru(string a)
        {
            string[] tehlikeliIfadeler = { "1=1", "--", ";", "@@", "@", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill", "open", "select", "sys", "sysobjects", "syscolumns", "table", "update", "\\", "\\n" };

            foreach (string ifade in tehlikeliIfadeler)
            {
                if (a.Contains(ifade))
                {
                    MessageBox.Show("SQL injection koruması devreye girdi . . .\n Tehlikeli ifade: " + ifade);

                    return false;
                }
            }
            return true;
        }
        static public string md5ilesifrele(string sifrelemelik)
        {
            StringBuilder sb = new StringBuilder();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //birinci sifreleme
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelemelik);
            dizi = md5.ComputeHash(dizi);
            foreach (var item in dizi)
                sb.Append(item.ToString("x2").ToLower());
            //ikinci sifreleme
            dizi = Encoding.UTF8.GetBytes(sb.ToString() + "50asbc");
            dizi = md5.ComputeHash(dizi);
            foreach (var item in dizi)
                sb.Append(item.ToString("x2").ToLower());
            //üçüncü sifreleme
            dizi = Encoding.UTF8.GetBytes(sb.ToString() + "mMaA9*3*/-+ğ<>db");
            dizi = md5.ComputeHash(dizi);
            foreach (var item in dizi)
                sb.Append(item.ToString("x2").ToLower());

            return sb.ToString();
        }



        public static string SayisiniGetir(string SyKomut, string SyParam, string SyKosul)
        {
            string görüntülenme = null;
            cmd = new NpgsqlCommand();
            con = new NpgsqlConnection(connectionString);
            cmd.Connection = con;
            if (SyKosul.All(c => char.IsDigit(c)))
                cmd.Parameters.AddWithValue(SyParam, Convert.ToInt32(SyKosul));
            else
                cmd.Parameters.AddWithValue(SyParam, SyKosul);
            con.Open();

            cmd.CommandText = SyKomut;
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                görüntülenme = rdr[0].ToString();
            }
            con.Close();

            return görüntülenme;
        }


        public static bool YetkiKontrol()
        {
            cmd = new NpgsqlCommand();
            con = new NpgsqlConnection(connectionString);
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "Select AdminYetkisi from Oturum where Ad='" + VideoOtomasyon.KullanıcınınAdı + "'";
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                if (rdr[0].ToString() == "True") return true;
                else if (rdr[0].ToString() == "True") return false;
            }

            return false;
        }

        public static void paramatrelidata(string komut1, string[] paramss)
        {
            //parametreler sırasıyla @0,@1,@2 olarak devam etmelidir
            //parametre değerleri ise string dizisi içinde aynı sırayla verilmedilir
            cmd = new NpgsqlCommand(komut1, con);
            for (int i = 0; i < paramss.Length; i++)
            {
                foreach (var a in paramss[i])
                    if (paramss[i].All(c => char.IsDigit(c)))
                        cmd.Parameters.AddWithValue("@" + i, Convert.ToInt32(paramss[i]));
                    else
                        cmd.Parameters.AddWithValue("@" + i, paramss[i]);
            }
            con.Open();
            rdr = cmd.ExecuteReader();
        }


    }

}
