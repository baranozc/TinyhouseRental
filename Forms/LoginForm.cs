using System;
using System.Windows.Forms;
using MyProject.Config;
using MyProject.Models;
using System.Drawing;
using System.Drawing.Drawing2D;

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
        private Label lblClient;
        private Label lblAdmin;
        private Panel pnlSwitch;
        private Panel pnlSlider;
        private bool isAdmin = false;

        public LoginForm()
        {
            InitializeComponent();
            // Başlangıçta slider sola ve renkler doğru olsun
            pnlSlider.Left = 2;
            lblClient.ForeColor = Color.FromArgb(76, 175, 80);
            lblAdmin.ForeColor = Color.FromArgb(100, 100, 100);
        }

        private void InitializeComponent()
        {
            this.txtEmail = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.btnSignUp = new Button();
            this.lblEmail = new Label();
            this.lblPassword = new Label();

            // Form
            this.Text = "Tiny House Rental - Login";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 245); // Hafif gri arka plan

            // Ortalamak için yardımcı değişkenler
            int centerX = (this.ClientSize.Width - 280) / 2; // 280: textbox ve buton genişliği
            int switchWidth = 180;
            int switchX = (this.ClientSize.Width - switchWidth) / 2;

            // Email Label
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(centerX, 100);
            this.lblEmail.AutoSize = true;

            // Email TextBox
            this.txtEmail.Location = new System.Drawing.Point(centerX, 120);
            this.txtEmail.Size = new System.Drawing.Size(280, 25);

            // Password Label
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(centerX, 160);
            this.lblPassword.AutoSize = true;

            // Password TextBox
            this.txtPassword.Location = new System.Drawing.Point(centerX, 180);
            this.txtPassword.Size = new System.Drawing.Size(280, 25);
            this.txtPassword.PasswordChar = '•';

            // Login Button
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point(centerX, 240);
            this.btnLogin.Size = new System.Drawing.Size(130, 35);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Click += new EventHandler(BtnLogin_Click);

            // Sign Up Button
            this.btnSignUp.Text = "Sign Up";
            this.btnSignUp.Location = new System.Drawing.Point(centerX + 150, 240);
            this.btnSignUp.Size = new System.Drawing.Size(130, 35);
            this.btnSignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSignUp.ForeColor = System.Drawing.Color.White;
            this.btnSignUp.FlatStyle = FlatStyle.Flat;
            this.btnSignUp.Click += new EventHandler(BtnSignUp_Click);

            // Switch Panel
            this.pnlSwitch = new Panel();
            this.pnlSwitch.Size = new System.Drawing.Size(switchWidth, 40);
            this.pnlSwitch.Location = new System.Drawing.Point(switchX, 40);
            this.pnlSwitch.BackColor = Color.White;
            this.pnlSwitch.BorderStyle = BorderStyle.None;
            this.pnlSwitch.Paint += (sender, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    path.AddArc(0, 0, 40, 40, 90, 180);
                    path.AddArc(140, 0, 40, 40, -90, 180);
                    path.CloseFigure();
                    using (var brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    using (var pen = new Pen(Color.LightGray, 2))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            // Client Label
            this.lblClient = new Label();
            this.lblClient.Text = "Client";
            this.lblClient.Size = new System.Drawing.Size(70, 40);
            this.lblClient.Location = new System.Drawing.Point(0, 0);
            this.lblClient.TextAlign = ContentAlignment.MiddleCenter;
            this.lblClient.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblClient.ForeColor = Color.FromArgb(76, 175, 80); // Yeşil
            this.lblClient.Cursor = Cursors.Hand;
            this.lblClient.BackColor = Color.Transparent;
            this.lblClient.Click += Switch_Click;

            // Admin Label
            this.lblAdmin = new Label();
            this.lblAdmin.Text = "Admin";
            this.lblAdmin.Size = new System.Drawing.Size(70, 40);
            this.lblAdmin.Location = new System.Drawing.Point(110, 0);
            this.lblAdmin.TextAlign = ContentAlignment.MiddleCenter;
            this.lblAdmin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAdmin.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblAdmin.Cursor = Cursors.Hand;
            this.lblAdmin.BackColor = Color.Transparent;
            this.lblAdmin.Click += Switch_Click;

            // Slider (yuvarlak)
            this.pnlSlider = new Panel();
            this.pnlSlider.Size = new System.Drawing.Size(36, 36);
            this.pnlSlider.Location = new System.Drawing.Point(2, 2);
            this.pnlSlider.BackColor = Color.Transparent;
            this.pnlSlider.Paint += (sender, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color sliderColor = isAdmin ? Color.FromArgb(33, 150, 243) : Color.FromArgb(76, 175, 80);
                using (var brush = new SolidBrush(sliderColor))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, 36, 36);
                }
            };
            this.pnlSlider.Cursor = Cursors.Hand;
            this.pnlSlider.Click += Switch_Click;

            // Switch Panel'e ekle (önce label'lar, en üste slider)
            this.pnlSwitch.Controls.Add(this.lblClient);
            this.pnlSwitch.Controls.Add(this.lblAdmin);
            this.pnlSwitch.Controls.Add(this.pnlSlider);

            // Add controls to form
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnSignUp);
            this.Controls.Add(this.pnlSwitch);
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

                try
                {
                    var user = User.FindByEmailAndPassword(email, password);
                    if (user == null)
                    {
                        MessageBox.Show("Email veya şifre hatalı.", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if ((!isAdmin && user.UserTypeId != UserTypeId.CLIENT) || (isAdmin && user.UserTypeId != UserTypeId.ADMIN))
                    {
                        MessageBox.Show(
                            isAdmin
                                ? "Yalnızca admin hesabı ile giriş yapabilirsiniz. Lütfen bir admin hesabı bilgisi girin."
                                : "Yalnızca client hesabı ile giriş yapabilirsiniz. Lütfen bir client hesabı bilgisi girin.",
                            "Kullanıcı Tipi Uyuşmazlığı",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show($"Hoşgeldiniz {user.Name} {user.Surname}!", "Giriş Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (user.UserTypeId == UserTypeId.CLIENT)
                    {
                        var clientDashboard = new ClientDashboard((Client)user);
                        this.Hide();
                        clientDashboard.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        var adminDashboard = new AdminDashboard((Admin)user);
                        this.Hide();
                        adminDashboard.ShowDialog();
                        this.Close();
                    }
                }
                catch (User.InactiveUserException)
                {
                    MessageBox.Show("Bu hesap admin tarafından dondurulmuştur.", "Hesap Donduruldu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
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

        private void Switch_Click(object sender, EventArgs e)
        {
            isAdmin = !isAdmin;
            if (isAdmin)
            {
                // Slider sağa
                pnlSlider.Left = pnlSwitch.Width - pnlSlider.Width - 2;
                lblAdmin.ForeColor = Color.FromArgb(33, 150, 243); // Mavi
                lblClient.ForeColor = Color.FromArgb(100, 100, 100);
            }
            else
            {
                // Slider sola
                pnlSlider.Left = 2;
                lblClient.ForeColor = Color.FromArgb(76, 175, 80); // Yeşil
                lblAdmin.ForeColor = Color.FromArgb(100, 100, 100);
            }
            pnlSlider.Invalidate();
        }
    }
} 