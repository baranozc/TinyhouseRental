using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace MyProject.Forms
{
    public partial class ImageViewerForm : Form
    {
        private PictureBox pictureBox;
        private Button btnPrevious;
        private Button btnNext;
        private Label lblImageIndex;
        private List<string> _imagePaths;
        private int _currentIndex;
        private bool _isAnimatedGif = false; // Görselin animasyonlu GIF olup olmadığını tutar

        public ImageViewerForm(List<string> imagePaths, int startIndex = 0)
        {
            _imagePaths = imagePaths;
            _currentIndex = startIndex;
            InitializeComponent();
            LoadImage(_imagePaths[_currentIndex]);
            UpdateImageIndexLabel();
        }

        private void InitializeComponent()
        {
            this.pictureBox = new PictureBox();
            this.btnPrevious = new Button();
            this.btnNext = new Button();
            this.lblImageIndex = new Label();
            this.SuspendLayout();

            // pictureBox
            this.pictureBox.Dock = DockStyle.Fill;
            this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new PaintEventHandler(PictureBox_Paint); // Paint olayını ekle

            // btnPrevious
            this.btnPrevious.Text = "<";
            this.btnPrevious.Size = new Size(50, 50);
            this.btnPrevious.Location = new Point(10, (this.ClientSize.Height - this.btnPrevious.Height) / 2);
            this.btnPrevious.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.btnPrevious.Click += new EventHandler(BtnPrevious_Click);

            // btnNext
            this.btnNext.Text = ">";
            this.btnNext.Size = new Size(50, 50);
            this.btnNext.Location = new Point(this.ClientSize.Width - this.btnNext.Width - 10, (this.ClientSize.Height - this.btnNext.Height) / 2);
            this.btnNext.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.btnNext.Click += new EventHandler(BtnNext_Click);

            // lblImageIndex
            this.lblImageIndex.AutoSize = true;
            this.lblImageIndex.Location = new Point((this.ClientSize.Width - this.lblImageIndex.Width) / 2, this.ClientSize.Height - 30);
            this.lblImageIndex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.lblImageIndex.TextAlign = ContentAlignment.MiddleCenter;

            // ImageViewerForm
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.lblImageIndex);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.pictureBox);
            this.Name = "ImageViewerForm";
            this.Text = "Görsel Görüntüleyici";
            this.Load += new System.EventHandler(this.ImageViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout(); // Konumlandırma için layout'u yeniden hesapla
        }

        private void ImageViewerForm_Load(object sender, System.EventArgs e)
        {
            // Form yüklendiğinde yapılacak ek ayarlar (gerekirse)
            // Kontrollerin konumunu güncelle
            this.btnPrevious.Location = new Point(10, (this.ClientSize.Height - this.btnPrevious.Height) / 2);
            this.btnNext.Location = new Point(this.ClientSize.Width - this.btnNext.Width - 10, (this.ClientSize.Height - this.btnNext.Height) / 2);
            this.lblImageIndex.Location = new Point((this.ClientSize.Width - this.lblImageIndex.Width) / 2, this.ClientSize.Height - this.lblImageIndex.Height - 10);
        }

        private void LoadImage(string imagePath)
        {
            // Önceki animasyonu durdur ve görseli dispose et
            StopAnimateAndDisposeImage();

            if (!System.IO.File.Exists(imagePath))
            {
                MessageBox.Show("Görsel dosyası bulunamadı: " + imagePath, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBox.Image = null; // Görseli temizle
                _isAnimatedGif = false;
                lblImageIndex.Text = "Hata";
                return; // İşlemi durdur
            }

            try
            {
                // Dosyadan görseli yükle
                Image image = Image.FromFile(imagePath);
                pictureBox.Image = image; // PictureBox'a görseli ata
                _isAnimatedGif = false; // Varsayılan olarak animasyonlu değil

                // Görselin animasyonlu olup olmadığını kontrol et
                if (ImageAnimator.CanAnimate(image) && image.GetFrameCount(FrameDimension.Time) > 1)
                {
                    _isAnimatedGif = true;
                    // Animasyonu başlat
                    ImageAnimator.Animate(image, OnFrameChanged);
                }
                
                // Form boyutunu görsele göre ayarla (isteğe bağlı)
                // this.Size = new Size(image.Width + 50, image.Height + 100); // Butonlar ve label için boşluk bırak
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Görsel yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBox.Image = null; // Hata durumunda görseli temizle
                _isAnimatedGif = false;
                lblImageIndex.Text = "Hata";
            }
        }

        // Animasyon karesi değiştiğinde çağrılır
        private void OnFrameChanged(object? sender, EventArgs e)
        {
            // PictureBox'ı yeniden çizmeye zorla
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(OnFrameChanged));
            }
            else
            {
                this.pictureBox.Invalidate();
            }
        }

        // PictureBox boyandığında animasyon karesini güncelle
        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            if (pictureBox.Image != null && _isAnimatedGif)
            {
                // Animasyonun güncel karesini güncelle
                ImageAnimator.UpdateFrames(pictureBox.Image);
            }
        }

        // Mevcut animasyonu durdur ve görseli dispose et
        private void StopAnimateAndDisposeImage()
        {
             // Animasyonu durdur
            if (pictureBox.Image != null && _isAnimatedGif)
            {
                ImageAnimator.StopAnimate(pictureBox.Image, OnFrameChanged);
            }
            // Görseli dispose et
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
             _isAnimatedGif = false; // Durumu sıfırla
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (_imagePaths == null || _imagePaths.Count == 0) return;

            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = _imagePaths.Count - 1; // Listenin sonuna dön
            }
            LoadImage(_imagePaths[_currentIndex]);
            UpdateImageIndexLabel();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_imagePaths == null || _imagePaths.Count == 0) return;

            _currentIndex++;
            if (_currentIndex >= _imagePaths.Count)
            {
                _currentIndex = 0; // Listenin başına dön
            }
            LoadImage(_imagePaths[_currentIndex]);
            UpdateImageIndexLabel();
        }

        private void UpdateImageIndexLabel()
        {
            if (_imagePaths != null && _imagePaths.Count > 0)
            {
                lblImageIndex.Text = $"{_currentIndex + 1} / {_imagePaths.Count}";
            }
            else
            {
                lblImageIndex.Text = "0 / 0";
            }
        }

        // Form kapatılırken mevcut animasyonu durdur ve görseli dispose et
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopAnimateAndDisposeImage(); // Yeni metodu kullan
        }
    }
} 