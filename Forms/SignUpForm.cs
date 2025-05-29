using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using MyProject.Config;
using MyProject.Models;

namespace MyProject.Forms
{
    public partial class SignUpForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtName;
        private TextBox txtSurname;
        private Button btnRegister;
        private Button btnCancel;
        private Label lblEmail;
        private Label lblPassword;
        private Label lblName;
        private Label lblSurname;

        public SignUpForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtEmail = new TextBox();
            this.txtPassword = new TextBox();
            this.txtName = new TextBox();
            this.txtSurname = new TextBox();
            this.btnRegister = new Button();
            this.btnCancel = new Button();
            this.lblEmail = new Label();
            this.lblPassword = new Label();
            this.lblName = new Label();
            this.lblSurname = new Label();

            // Form
            this.Text = "Tiny House Rental - Sign Up";
            this.Size = new System.Drawing.Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Name Label
            this.lblName.Text = "Name:";
            this.lblName.Location = new System.Drawing.Point(50, 30);
            this.lblName.AutoSize = true;

            // Name TextBox
            this.txtName.Location = new System.Drawing.Point(50, 50);
            this.txtName.Size = new System.Drawing.Size(280, 25);

            // Surname Label
            this.lblSurname.Text = "Surname:";
            this.lblSurname.Location = new System.Drawing.Point(50, 90);
            this.lblSurname.AutoSize = true;

            // Surname TextBox
            this.txtSurname.Location = new System.Drawing.Point(50, 110);
            this.txtSurname.Size = new System.Drawing.Size(280, 25);

            // Email Label
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(50, 150);
            this.lblEmail.AutoSize = true;

            // Email TextBox
            this.txtEmail.Location = new System.Drawing.Point(50, 170);
            this.txtEmail.Size = new System.Drawing.Size(280, 25);

            // Password Label
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(50, 210);
            this.lblPassword.AutoSize = true;

            // Password TextBox
            this.txtPassword.Location = new System.Drawing.Point(50, 230);
            this.txtPassword.Size = new System.Drawing.Size(280, 25);
            this.txtPassword.PasswordChar = '•';

            // Register Button
            this.btnRegister.Text = "Register";
            this.btnRegister.Location = new System.Drawing.Point(50, 280);
            this.btnRegister.Size = new System.Drawing.Size(130, 35);
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.FlatStyle = FlatStyle.Flat;
            this.btnRegister.Click += new EventHandler(BtnRegister_Click);

            // Cancel Button
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(200, 280);
            this.btnCancel.Size = new System.Drawing.Size(130, 35);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Add controls to form
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblSurname);
            this.Controls.Add(this.txtSurname);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnCancel);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || 
                string.IsNullOrWhiteSpace(txtSurname.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text) || 
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Users (email, password, name, surname, userTypeID, userState)
                        VALUES (@Email, @Password, @Name, @Surname, 1, 1)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);
                        command.Parameters.AddWithValue("@Password", txtPassword.Text);
                        command.Parameters.AddWithValue("@Name", txtName.Text);
                        command.Parameters.AddWithValue("@Surname", txtSurname.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Registration successful! You can now login.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during registration: {ex.Message}", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
} 