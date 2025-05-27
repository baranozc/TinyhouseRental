using System;
using System.Windows.Forms;
using MyProject.Config;
using MyProject.Models;

namespace MyProject.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblEmail;
        private Label lblPassword;
        private RadioButton rbClient;
        private RadioButton rbAdmin;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtEmail = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.lblEmail = new Label();
            this.lblPassword = new Label();
            this.rbClient = new RadioButton();
            this.rbAdmin = new RadioButton();

            // Form
            this.Text = "Tiny House Rental - Login";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Email Label
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(50, 50);
            this.lblEmail.AutoSize = true;

            // Email TextBox
            this.txtEmail.Location = new System.Drawing.Point(150, 50);
            this.txtEmail.Size = new System.Drawing.Size(200, 20);

            // Password Label
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(50, 90);
            this.lblPassword.AutoSize = true;

            // Password TextBox
            this.txtPassword.Location = new System.Drawing.Point(150, 90);
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.PasswordChar = '*';

            // Radio Buttons
            this.rbClient.Text = "Client";
            this.rbClient.Location = new System.Drawing.Point(150, 130);
            this.rbClient.Checked = true;

            this.rbAdmin.Text = "Admin";
            this.rbAdmin.Location = new System.Drawing.Point(250, 130);

            // Login Button
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point(150, 170);
            this.btnLogin.Size = new System.Drawing.Size(100, 30);
            this.btnLogin.Click += new EventHandler(BtnLogin_Click);

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                this.lblEmail,
                this.txtEmail,
                this.lblPassword,
                this.txtPassword,
                this.rbClient,
                this.rbAdmin,
                this.btnLogin
            });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Lütfen email ve şifre alanlarını doldurun.", "Giriş Hatası", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UserResponse response;
                if (rbClient.Checked)
                {
                    var client = new Client();
                    response = client.Login(email, password);
                }
                else
                {
                    var admin = new Admin();
                    response = admin.Login(email, password);
                }

                if (response.Success)
                {
                    MessageBox.Show($"Hoşgeldiniz {response.User.Name} {response.User.Surname}!", "Giriş Başarılı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if (response.User.UserTypeId == UserTypeId.CLIENT)
                    {
                        var clientDashboard = new ClientDashboard((Client)response.User);
                        this.Hide();
                        clientDashboard.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        var adminDashboard = new AdminDashboard((Admin)response.User);
                        this.Hide();
                        adminDashboard.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show($"Giriş başarısız: {response.Message}", "Giriş Hatası", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                MessageBox.Show($"Veritabanı hatası: {sqlEx.Message}\n\nBağlantı dizesi: {DatabaseConfig.ConnectionString}", 
                    "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}\n\nHata detayı: {ex.StackTrace}", 
                    "Sistem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 