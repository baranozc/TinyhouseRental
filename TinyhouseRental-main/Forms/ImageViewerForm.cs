using System.Drawing;
using System.Windows.Forms;

namespace MyProject.Forms
{
    public partial class ImageViewerForm : Form
    {
        private PictureBox pictureBox;

        public ImageViewerForm(string imagePath)
        {
            InitializeComponent();
            LoadImage(imagePath);
        }

        private void InitializeComponent()
        {
            this.pictureBox = new PictureBox();
            this.SuspendLayout();

            // pictureBox
            this.pictureBox.Dock = DockStyle.Fill;
            this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;

            // ImageViewerForm
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.pictureBox);
            this.Name = "ImageViewerForm";
            this.Text = "Görsel Görüntüleyici";
            this.Load += new System.EventHandler(this.ImageViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
        }

        private void ImageViewerForm_Load(object sender, System.EventArgs e)
        {
            // Form yüklendiğinde yapılacak ek ayarlar (gerekirse)
        }

        private void LoadImage(string imagePath)
        {
            try
            {
                if (System.IO.File.Exists(imagePath))
                {
                    // Dosyadan görseli yükle
                    pictureBox.Image = Image.FromFile(imagePath);
                }
                else
                {
                    MessageBox.Show("Görsel dosyası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Görsel yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
} 