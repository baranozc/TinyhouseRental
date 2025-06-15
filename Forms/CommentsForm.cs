using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MyProject.Config;
using MyProject.Models;
using System.Drawing;

namespace MyProject.Forms
{
    public partial class CommentsForm : Form
    {
        private int _listingId;
        private string _listingTitle;
        private DataGridView dgvComments;
        private Button btnAddComment;
        private Button btnClose;
        private TextBox txtNewComment;
        private Label lblComments;

        public CommentsForm(int listingId, string listingTitle)
        {
            _listingId = listingId;
            _listingTitle = listingTitle;
            InitializeComponent();
            LoadComments();
        }

        private void InitializeComponent()
        {
            this.dgvComments = new DataGridView();
            this.btnAddComment = new Button();
            this.btnClose = new Button();
            this.txtNewComment = new TextBox();
            this.lblComments = new Label();

            // Form
            this.Text = $"Comments - {_listingTitle}";
            this.Size = new System.Drawing.Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            // Comments Label
            this.lblComments.Text = "Comments";
            this.lblComments.Location = new System.Drawing.Point(20, 20);
            this.lblComments.AutoSize = true;
            this.lblComments.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);

            // Comments DataGridView
            this.dgvComments.Location = new System.Drawing.Point(20, 60);
            this.dgvComments.Size = new System.Drawing.Size(540, 300);
            this.dgvComments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvComments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvComments.MultiSelect = false;
            this.dgvComments.BackgroundColor = System.Drawing.Color.White;
            this.dgvComments.BorderStyle = BorderStyle.None;
            this.dgvComments.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvComments.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvComments.EnableHeadersVisualStyles = false;
            this.dgvComments.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dgvComments.RowHeadersVisible = false;
            this.dgvComments.AllowUserToAddRows = false;
            this.dgvComments.ReadOnly = true;

            // New Comment TextBox
            this.txtNewComment.Location = new System.Drawing.Point(20, 380);
            this.txtNewComment.Size = new System.Drawing.Size(540, 30);
            this.txtNewComment.Multiline = true;
            this.txtNewComment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            // Add Comment Button
            this.btnAddComment.Text = "Add Comment";
            this.btnAddComment.Location = new System.Drawing.Point(20, 420);
            this.btnAddComment.Size = new System.Drawing.Size(120, 35);
            this.btnAddComment.FlatStyle = FlatStyle.Flat;
            this.btnAddComment.BackColor = Color.FromArgb(30, 41, 59);
            this.btnAddComment.ForeColor = Color.White;
            this.btnAddComment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAddComment.Cursor = Cursors.Hand;
            this.btnAddComment.Click += new EventHandler(BtnAddComment_Click);

            // Close Button
            this.btnClose.Text = "Close";
            this.btnClose.Location = new System.Drawing.Point(440, 420);
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.BackColor = Color.FromArgb(239, 68, 68);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.Cursor = Cursors.Hand;
            this.btnClose.Click += new EventHandler(BtnClose_Click);

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                this.lblComments,
                this.dgvComments,
                this.txtNewComment,
                this.btnAddComment,
                this.btnClose
            });
        }

        private void LoadComments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT c.CommentID, c.CommentContent, c.CreatedDate, u.Name, u.Surname 
                                   FROM Comments c 
                                   INNER JOIN Users u ON c.UserID = u.UserID 
                                   WHERE c.ListingID = @ListingID 
                                   ORDER BY c.CreatedDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ListingID", _listingId);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvComments.DataSource = dataTable;
                        dgvComments.Columns["CommentID"].Visible = false;
                        dgvComments.Columns["CommentContent"].HeaderText = "Comment";
                        dgvComments.Columns["CreatedDate"].HeaderText = "Date";
                        dgvComments.Columns["Name"].HeaderText = "User";
                        dgvComments.Columns["Surname"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading comments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddComment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewComment.Text))
            {
                MessageBox.Show("Please enter a comment.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Comments (ListingID, UserID, CommentContent, CreatedDate) 
                                   VALUES (@ListingID, @UserID, @CommentContent, @CreatedDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ListingID", _listingId);
                        command.Parameters.AddWithValue("@UserID", Program.CurrentUser.UserID);
                        command.Parameters.AddWithValue("@CommentContent", txtNewComment.Text);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        command.ExecuteNonQuery();
                        txtNewComment.Clear();
                        LoadComments();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding comment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
} 