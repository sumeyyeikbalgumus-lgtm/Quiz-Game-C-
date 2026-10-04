using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

// VS Code'un Timer kütüphanelerini karıştırmaması için açık alias tanımı:
using FormsTimer = System.Windows.Forms.Timer;

namespace QuizUygulamasi
{
    // Soru mimarisini nesne yönelimli hale getiren Soru sınıfı
    public class Soru
    {
        public string SoruMetni { get; set; }
        public string SecenekA { get; set; }
        public string SecenekB { get; set; }
        public string SecenekC { get; set; }
        public string SecenekD { get; set; }
        public string DogruCevap { get; set; }

        public Soru(string soruMetni, string a, string b, string c, string d, string dogruCevap)
        {
            SoruMetni = soruMetni;
            SecenekA = a;
            SecenekB = b;
            SecenekC = c;
            SecenekD = d;
            DogruCevap = dogruCevap;
        }
    }

    public class AnaForm : Form
    {
        private List<Yarismaci> yarismacilar = new List<Yarismaci>();
        private List<Soru> sorular = new List<Soru>();

        private Yarismaci mevcutYarismaci;
        private int soruIndex = 0;

        // Arayüz Elemanları
        private Label lblHosgeldiniz = new Label();
        private Label lblBilgi = new Label();
        private TextBox txtAd = new TextBox();
        private Button btnBasla = new Button();

        private Label lblSoru = new Label();
        private Button btnA = new Button();
        private Button btnB = new Button();
        private Button btnC = new Button();
        private Button btnD = new Button();

        private Label lblSonuc = new Label();
        private DataGridView dgvTablo = new DataGridView();
        private Button btnYenidenOyna = new Button();

        public AnaForm()
        {
            FormAyarlariniYap();
            SorulariYukle();
            BilesenleriIlklendir();
        }

        private void FormAyarlariniYap()
        {
            this.Text = "BİLGİ YARIŞMASI";
            this.Size = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(20, 20, 20); // Koyu tema
        }

        private void SorulariYukle()
        {
            sorular.Clear();
            sorular.Add(new Soru("1) Hangisi bir programlama dilidir?", "Python", "VS Code", "GitHub", "Google", "Python"));
            sorular.Add(new Soru("2) Hangisi bir web tarayıcısıdır?", "C#", "Firefox", "Ruby", "DevC++", "Firefox"));
            sorular.Add(new Soru("3) Hangisi bir veri tabanı yönetim sistemidir?", "MySQL", "Visual Studio", "Git", "Photoshop", "MySQL"));
        }

        private void BilesenleriIlklendir()
        {
            // Giriş Ekranı Elemanları
            lblHosgeldiniz.Text = "Bilgi Yarışmasına Hoş Geldiniz!";
            lblHosgeldiniz.ForeColor = Color.Beige;
            lblHosgeldiniz.Font = new Font("Georgia", 20, FontStyle.Bold);
            lblHosgeldiniz.Location = new Point(50, 40);
            lblHosgeldiniz.AutoSize = true;

            lblBilgi.Text = "İsminizi yazdıktan sonra yarışmaya başlayabilirsiniz:";
            lblBilgi.ForeColor = Color.Beige;
            lblBilgi.Location = new Point(50, 110);
            lblBilgi.Font = new Font("Georgia", 12, FontStyle.Regular);
            lblBilgi.AutoSize = true;

            txtAd.BackColor = Color.Beige;
            txtAd.ForeColor = Color.Black;
            txtAd.BorderStyle = BorderStyle.FixedSingle;
            txtAd.Font = new Font("Arial", 12, FontStyle.Bold);
            txtAd.Location = new Point(50, 150);
            txtAd.Width = 480;

            btnBasla.Text = "BAŞLA >>>";
            btnBasla.Location = new Point(190, 220);
            btnBasla.Size = new Size(200, 50);
            btnBasla.BackColor = Color.Beige;
            btnBasla.ForeColor = Color.Black;
            btnBasla.Font = new Font("Georgia", 14, FontStyle.Bold);
            btnBasla.FlatStyle = FlatStyle.Flat;
            btnBasla.Click += BtnBasla_Click;

            // Soru Ekranı Elemanları
            lblSoru.ForeColor = Color.Beige;
            lblSoru.Location = new Point(50, 40);
            lblSoru.Size = new Size(480, 80);
            lblSoru.Font = new Font("Georgia", 13, FontStyle.Bold);
            lblSoru.Visible = false;

            ButonHazirla(btnA, "A", 140);
            ButonHazirla(btnB, "B", 210);
            ButonHazirla(btnC, "C", 280);
            ButonHazirla(btnD, "D", 350);

            // Sonuç Ekranı Elemanları
            lblSonuc.ForeColor = Color.Beige;
            lblSonuc.Font = new Font("Georgia", 13, FontStyle.Bold);
            lblSonuc.Location = new Point(50, 20);
            lblSonuc.Size = new Size(480, 110);
            lblSonuc.Visible = false;

            dgvTablo.Location = new Point(50, 140);
            dgvTablo.Size = new Size(480, 220);
            dgvTablo.Visible = false;
            TabloStiliniAyarla();

            btnYenidenOyna.Text = "Yeniden Oyna";
            btnYenidenOyna.Location = new Point(190, 380);
            btnYenidenOyna.Size = new Size(200, 45);
            btnYenidenOyna.BackColor = Color.Beige;
            btnYenidenOyna.Font = new Font("Georgia", 12, FontStyle.Bold);
            btnYenidenOyna.FlatStyle = FlatStyle.Flat;
            btnYenidenOyna.Visible = false;
            btnYenidenOyna.Click += BtnYenidenOyna_Click;

            // Kontrolleri Forma Ekleme
            this.Controls.Add(lblHosgeldiniz);
            this.Controls.Add(lblBilgi);
            this.Controls.Add(txtAd);
            this.Controls.Add(btnBasla);
            this.Controls.Add(lblSoru);
            this.Controls.Add(lblSonuc);
            this.Controls.Add(dgvTablo);
            this.Controls.Add(btnYenidenOyna);
        }

        private void ButonHazirla(Button btn, string etiket, int y)
        {
            btn.Text = etiket;
            btn.Location = new Point(50, y);
            btn.Size = new Size(480, 55);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.Font = new Font("Arial", 11, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Visible = false;
            btn.Click += CevapVer;
            this.Controls.Add(btn);
        }

        private void TabloStiliniAyarla()
        {
            dgvTablo.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvTablo.DefaultCellStyle.BackColor = Color.Beige;
            dgvTablo.DefaultCellStyle.ForeColor = Color.Black;
            dgvTablo.DefaultCellStyle.SelectionBackColor = Color.DarkSeaGreen;
            dgvTablo.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvTablo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTablo.ReadOnly = true;
            dgvTablo.RowHeadersVisible = false;
        }

        private void BtnBasla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text))
            {
                MessageBox.Show(this, "Lütfen adınızı giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            mevcutYarismaci = new Yarismaci { Ad = txtAd.Text.Trim() };

            GirisEkraniniGizle();
            SoruEkraniniGoster();

            soruIndex = 0;
            SoruGetir();
        }

        private void SoruGetir()
        {
            ButonlariSifirla();

            if (soruIndex < sorular.Count)
            {
                Soru mevcutSoru = sorular[soruIndex];
                lblSoru.Text = mevcutSoru.SoruMetni;
                btnA.Text = mevcutSoru.SecenekA;
                btnB.Text = mevcutSoru.SecenekB;
                btnC.Text = mevcutSoru.SecenekC;
                btnD.Text = mevcutSoru.SecenekD;
            }
            else
            {
                SonucGoster();
            }
        }

        private void CevapVer(object sender, EventArgs e)
        {
            Button basilanButon = (Button)sender;
            Soru aktifSoru = sorular[soruIndex];

            ButonlariAktiflikDurumu(false);

            if (basilanButon.Text == aktifSoru.DogruCevap)
            {
                basilanButon.BackColor = Color.LimeGreen;
                mevcutYarismaci.PuanArtir(); // Her doğru cevap için 33.33 puan ekler
            }
            else
            {
                basilanButon.BackColor = Color.IndianRed;
                DogruCevabiIsaretle(aktifSoru.DogruCevap);
            }

            FormsTimer timer = new FormsTimer();
            timer.Interval = 900;
            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                timer.Dispose();
                ButonlariAktiflikDurumu(true);
                soruIndex++;
                SoruGetir();
            };
            timer.Start();
        }

        private void DogruCevabiIsaretle(string dogruCevap)
        {
            if (btnA.Text == dogruCevap) btnA.BackColor = Color.LimeGreen;
            if (btnB.Text == dogruCevap) btnB.BackColor = Color.LimeGreen;
            if (btnC.Text == dogruCevap) btnC.BackColor = Color.LimeGreen;
            if (btnD.Text == dogruCevap) btnD.BackColor = Color.LimeGreen;
        }

        private void SonucGoster()
        {
            yarismacilar.Add(mevcutYarismaci);

            SoruEkraniniGizle();

            int dogruSayisi = (int)Math.Round(mevcutYarismaci.Puan / 33.33);

            lblSonuc.Text = $"Tebrikler, {mevcutYarismaci.Ad}!\n\n" +
                            $"{sorular.Count} sorudan {dogruSayisi} tanesini doğru bildiniz.\n" +
                            $"Toplam Puanınız: {mevcutYarismaci.PuanMetni}\n\n" +
                            $"Yarışmaya katıldığınız için teşekkürler!";
            lblSonuc.Visible = true;

            dgvTablo.DataSource = null;
            dgvTablo.DataSource = yarismacilar;
            dgvTablo.Visible = true;

            btnYenidenOyna.Visible = true;
        }

        private void BtnYenidenOyna_Click(object sender, EventArgs e)
        {
            lblSonuc.Visible = false;
            dgvTablo.Visible = false;
            btnYenidenOyna.Visible = false;

            txtAd.Text = "";
            lblHosgeldiniz.Visible = true;
            lblBilgi.Visible = true;
            txtAd.Visible = true;
            btnBasla.Visible = true;
        }

        private void ButonlariSifirla()
        {
            btnA.BackColor = Color.White;
            btnB.BackColor = Color.White;
            btnC.BackColor = Color.White;
            btnD.BackColor = Color.White;
        }

        private void ButonlariAktiflikDurumu(bool durum)
        {
            btnA.Enabled = durum;
            btnB.Enabled = durum;
            btnC.Enabled = durum;
            btnD.Enabled = durum;
        }

        private void GirisEkraniniGizle()
        {
            lblHosgeldiniz.Visible = false;
            lblBilgi.Visible = false;
            txtAd.Visible = false;
            btnBasla.Visible = false;
        }

        private void SoruEkraniniGoster()
        {
            lblSoru.Visible = true;
            btnA.Visible = true;
            btnB.Visible = true;
            btnC.Visible = true;
            btnD.Visible = true;
        }

        private void SoruEkraniniGizle()
        {
            lblSoru.Visible = false;
            btnA.Visible = false;
            btnB.Visible = false;
            btnC.Visible = false;
            btnD.Visible = false;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AnaForm());
        }
    }

    public class Yarismaci
    {
        private double _puan;

        [DisplayName("Yarışmacı Adı")]
        public string Ad { get; set; }

        [Browsable(false)] // Ham double puan bilgisini tablodan gizler
        public double Puan
        {
            get { return _puan; }
            set { _puan = value; }
        }

        [DisplayName("Toplam Puan")]
        public string PuanMetni
        {
            get
            {
                // 3 sorunun hepsi doğruysa tam 100 gösterir, aksi halde 33.33 veya 66.66 gösterir
                if (_puan >= 99.9)
                    return "100";

                return _puan.ToString("0.00");
            }
        }

        public void PuanArtir()
        {
            _puan += 33.33;
        }
    }
}