using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoOtomasyon
{
    public partial class RaporPenceresi : Form
    {
        public RaporPenceresi()
        {
            InitializeComponent();
        }

        private void reportDocument1_InitReport(object sender, EventArgs e)
        {

        }
        public static int RaporSeç = 0;
        private void RaporPenceresi_Load(object sender, EventArgs e)
        {
            OturumReport a = new OturumReport();
            a.SetDataSource(Fonksiyonlar.DatasetimiziDoldur("Select * from Oturum"));
            VideoReport b = new VideoReport();
            b.SetDataSource(Fonksiyonlar.DatasetimiziDoldur("Select * from Videolar "));
            GörüntülemelerReport c = new GörüntülemelerReport();
            c.SetDataSource(Fonksiyonlar.DatasetimiziDoldur("Select * from Görüntülenmeler "));
            YorumlarReport d = new YorumlarReport();
            d.SetDataSource(Fonksiyonlar.DatasetimiziDoldur("Select * from Yorumlar "));
            AboneliklerReport f = new AboneliklerReport();
            f.SetDataSource(Fonksiyonlar.DatasetimiziDoldur("Select * from Abonelikler"));
            if (RaporSeç==0)
            crystalReportViewer1.ReportSource = a;
            if (RaporSeç == 1)
            crystalReportViewer1.ReportSource = b;
            if (RaporSeç == 2)
                crystalReportViewer1.ReportSource = c;
            if (RaporSeç == 3)
                crystalReportViewer1.ReportSource = d;
            if (RaporSeç == 4)
                crystalReportViewer1.ReportSource = f;

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
