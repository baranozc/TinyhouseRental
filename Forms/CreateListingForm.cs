using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using MyProject.Config;
using MyProject.Models;
using System.IO;
using System.Collections.Generic;

namespace MyProject.Forms
{
    public partial class CreateListingForm : Form
    {
        private Client _client;
        private TextBox txtTitle;
        private TextBox txtDescription;
        private NumericUpDown numPrice;
        private Button btnCreate;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblPrice;
        private Button btnAddImage;
        private Label lblSelectedImage;
        private List<string> _selectedImagePaths;

        public CreateListingForm(Client client)
        {
            _client = client;
            _selectedImagePaths = new List<string>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtTitle = new TextBox();
            this.txtDescription = new TextBox();
            this.numPrice = new NumericUpDown();
            this.btnCreate = new Button();
            this.btnCancel = new Button();
            this.lblTitle = new Label();
            this.lblDescription = new Label();
            this.lblPrice = new Label();
            this.btnAddImage = new Button();
            this.lblSelectedImage = new Label();

            // Form
            this.Text = "Create New Listing";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Labels
            this.lblTitle.Text = "Title:";
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Size = new System.Drawing.Size(100, 23);
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);

            this.lblDescription.Text = "Description:";
            this.lblDescription.Location = new System.Drawing.Point(30, 90);
            this.lblDescription.Size = new System.Drawing.Size(100, 23);
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);

            this.lblPrice.Text = "Price (₺):";
            this.lblPrice.Location = new System.Drawing.Point(30, 220);
            this.lblPrice.Size = new System.Drawing.Size(100, 23);
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);

            // Image Upload
            this.btnAddImage.Text = "Görsel Ekle";
            this.btnAddImage.Location = new System.Drawing.Point(30, 280);
            this.btnAddImage.Size = new System.Drawing.Size(150, 35);
            this.btnAddImage.BackColor = System.Drawing.Color.FromArgb(52, 152, 219); // Mavi
            this.btnAddImage.ForeColor = System.Drawing.Color.White;
            this.btnAddImage.FlatStyle = FlatStyle.Flat;
            this.btnAddImage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddImage.Click += new EventHandler(BtnAddImage_Click);

            this.lblSelectedImage.Location = new System.Drawing.Point(200, 285);
            this.lblSelectedImage.Size = new System.Drawing.Size(250, 23);
            this.lblSelectedImage.Text = "Seçilen görsel yok";
            this.lblSelectedImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);

            // TextBoxes and NumericUpDown
            this.txtTitle.Location = new System.Drawing.Point(30, 55);
            this.txtTitle.Size = new System.Drawing.Size(420, 25);
            this.txtTitle.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.txtDescription.Location = new System.Drawing.Point(30, 115);
            this.txtDescription.Size = new System.Drawing.Size(420, 80);
            this.txtDescription.Multiline = true;
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.numPrice.Location = new System.Drawing.Point(30, 245);
            this.numPrice.Size = new System.Drawing.Size(150, 25);
            this.numPrice.Maximum = 100000;
            this.numPrice.Minimum = 0;
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Buttons
            this.btnCreate.Text = "Create Listing";
            this.btnCreate.Location = new System.Drawing.Point(30, 340);
            this.btnCreate.Size = new System.Drawing.Size(150, 35);
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.FlatStyle = FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreate.Click += new EventHandler(BtnCreate_Click);

            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(200, 340);
            this.btnCancel.Size = new System.Drawing.Size(150, 35);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                this.lblTitle,
                this.lblDescription,
                this.lblPrice,
                this.txtTitle,
                this.txtDescription,
                this.numPrice,
                this.btnAddImage,
                this.lblSelectedImage,
                this.btnCreate,
                this.btnCancel
            });
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtDescription.Text) || _selectedImagePaths.Count == 0)
            {
                MessageBox.Show("Please fill every requiring space and select an Image.", "Create Listing",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Görselleri kaydet ve yollarını topla
                List<string> savedImageUrls = new List<string>();
                string imagesDirectory = Path.Combine(Application.StartupPath, "Images");
                Directory.CreateDirectory(imagesDirectory); // Klasörü oluştur

                foreach (string selectedPath in _selectedImagePaths)
                {
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(selectedPath);
                    string destinationPath = Path.Combine(imagesDirectory, imageName);
                    File.Copy(selectedPath, destinationPath);
                    savedImageUrls.Add("Images/" + imageName); // Veritabanına göreceli yolu ekle
                }

                // Görsel yollarını virgülle birleştir
                string imageUrlsString = string.Join(",", savedImageUrls);

                // Veritabanına kaydet
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Listings (userID, listingTitle, listingDescription, rentalPrice, listingState, ImageUrl)
                        VALUES (@UserID, @Title, @Description, @Price, 1, @ImageUrl)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", _client.UserID);
                        command.Parameters.AddWithValue("@Title", txtTitle.Text);
                        command.Parameters.AddWithValue("@Description", txtDescription.Text);
                        command.Parameters.AddWithValue("@Price", numPrice.Value);
                        command.Parameters.AddWithValue("@ImageUrl", imageUrlsString); // Virgülle ayrılmış yolları kaydet

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Listing created successfully!", "Create Listing",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating listing: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.Title = "Select an Image!";
            openFileDialog.Multiselect = true; // Birden fazla seçime izin ver

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Seçilen dosyaların yollarını listeye ekle
                _selectedImagePaths.AddRange(openFileDialog.FileNames);
                // Seçilen dosya sayısını label'da göster
                lblSelectedImage.Text = $"{_selectedImagePaths.Count} images selected.";
            }
        }
    }
} 