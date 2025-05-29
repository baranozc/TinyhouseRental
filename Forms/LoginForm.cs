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
        private Button btnSignUp;
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
            this.btnSignUp = new Button();
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
            this.MinimizeBox = false;

            // Email Label
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(50, 50);
            this.lblEmail.AutoSize = true;

            // Email TextBox
            this.txtEmail.Location = new System.Drawing.Point(50, 70);
            this.txtEmail.Size = new System.Drawing.Size(280, 25);

            // Password Label
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(50, 100);
            this.lblPassword.AutoSize = true;

            // Password TextBox
            this.txtPassword.Location = new System.Drawing.Point(50, 120);
            this.txtPassword.Size = new System.Drawing.Size(280, 25);
            this.txtPassword.PasswordChar = '•';

            // Radio Buttons
            this.rbClient.Text = "Client";
            this.rbClient.Location = new System.Drawing.Point(50, 160);
            this.rbClient.Checked = true;

            this.rbAdmin.Text = "Admin";
            this.rbAdmin.Location = new System.Drawing.Point(150, 160);

            // Login Button
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point(50, 200);
            this.btnLogin.Size = new System.Drawing.Size(130, 35);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Click += new EventHandler(BtnLogin_Click);

            // Sign Up Button
            this.btnSignUp.Text = "Sign Up";
            this.btnSignUp.Location = new System.Drawing.Point(200, 200);
            this.btnSignUp.Size = new System.Drawing.Size(130, 35);
            this.btnSignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSignUp.ForeColor = System.Drawing.Color.White;
            this.btnSignUp.FlatStyle = FlatStyle.Flat;
            this.btnSignUp.Click += new EventHandler(BtnSignUp_Click);

            // Add controls to form
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.rbClient);
            this.Controls.Add(this.rbAdmin);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnSignUp);
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

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            var signUpForm = new SignUpForm();
            this.Hide();
            signUpForm.ShowDialog();
            this.Show();
        }
    }
} 