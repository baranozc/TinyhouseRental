using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MyProject.Config;
using MyProject.Models;

namespace MyProject.Forms
{
    public partial class ClientDashboard : Form
    {
        private Client _client;
        private TabControl tabControl;
        private TabPage tabListings;
        private TabPage tabReservations;
        private TabPage tabMyListings;
        private TabPage tabProfile;
        private DataGridView dgvListings;
        private DataGridView dgvReservations;
        private DataGridView dgvMyListings;
        private Button btnMakeReservation;
        private Button btnViewDetails;
        private Button btnCancelReservation;
        private Button btnCreateListing;
        private Button btnLogout;
        private Button btnSwitchAccount;
        private TextBox txtName;
        private TextBox txtSurname;
        private TextBox txtOldPassword;
        private TextBox txtNewPassword;
        private TextBox txtNewPasswordRepeat;
        private TextBox txtAbout;
        private Button btnSaveProfile;
        private Label lblProfileInfo;
        private Label lblName;
        private Label lblSurname;
        private Label lblOldPassword;
        private Label lblNewPassword;
        private Label lblNewPasswordRepeat;
        private Label lblAbout;
        private Panel pnlProfileCard;
        private PictureBox picProfileIcon;
        private Label lblProfileTitle;
        private TableLayoutPanel tblProfileLayout;

        public ClientDashboard(Client client)
        {
            _client = client;
            InitializeComponent();
            LoadListings();
            LoadReservations();
            LoadMyListings();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabListings = new TabPage();
            this.tabReservations = new TabPage();
            this.tabMyListings = new TabPage();
            this.tabProfile = new TabPage();
            this.dgvListings = new DataGridView();
            this.dgvReservations = new DataGridView();
            this.dgvMyListings = new DataGridView();
            this.btnMakeReservation = new Button();
            this.btnViewDetails = new Button();
            this.btnCancelReservation = new Button();
            this.btnCreateListing = new Button();
            this.btnLogout = new Button();
            this.btnSwitchAccount = new Button();
            this.txtName = new TextBox();
            this.txtSurname = new TextBox();
            this.txtOldPassword = new TextBox();
            this.txtNewPassword = new TextBox();
            this.txtNewPasswordRepeat = new TextBox();
            this.txtAbout = new TextBox();
            this.btnSaveProfile = new Button();
            this.lblProfileInfo = new Label();
            this.lblName = new Label();
            this.lblSurname = new Label();
            this.lblOldPassword = new Label();
            this.lblNewPassword = new Label();
            this.lblNewPasswordRepeat = new Label();
            this.lblAbout = new Label();
            this.pnlProfileCard = new Panel();
            this.picProfileIcon = new PictureBox();
            this.lblProfileTitle = new Label();

            // Form
            this.Text = "Tiny House Rental - Client Dashboard";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            // Logout Button
            this.btnLogout.Text = "Çıkış Yap";
            this.btnLogout.Size = new System.Drawing.Size(100, 35);
            this.btnLogout.Location = new System.Drawing.Point(this.Width - btnLogout.Width - 25, 10);
            this.btnLogout.FlatStyle = FlatStyle.Flat;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.Cursor = Cursors.Hand;
            this.btnLogout.Click += new EventHandler(BtnLogout_Click);
            this.btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Hesap Değiştir Butonu
            this.btnSwitchAccount.Text = "Hesap Değiştir";
            this.btnSwitchAccount.Size = new System.Drawing.Size(120, 35);
            this.btnSwitchAccount.Location = new System.Drawing.Point(this.Width - btnLogout.Width - btnSwitchAccount.Width - 35, 10);
            this.btnSwitchAccount.FlatStyle = FlatStyle.Flat;
            this.btnSwitchAccount.BackColor = System.Drawing.Color.FromArgb(33, 150, 243); // Mavi
            this.btnSwitchAccount.ForeColor = System.Drawing.Color.White;
            this.btnSwitchAccount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSwitchAccount.Cursor = Cursors.Hand;
            this.btnSwitchAccount.Click += new EventHandler(BtnSwitchAccount_Click);
            this.btnSwitchAccount.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Tab Control - Adjust position to make room for logout button
            this.tabControl.Location = new Point(0, 50);
            this.tabControl.Size = new System.Drawing.Size(this.Width, this.Height - 50);
            this.tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tabControl.Padding = new Point(10, 5);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tabControl.Controls.AddRange(new Control[] {
                this.tabListings,
                this.tabReservations,
                this.tabMyListings,
                this.tabProfile
            });

            // Tab Pages
            this.tabListings.Text = "Available Listings";
            this.tabListings.BackColor = System.Drawing.Color.White;
            this.tabListings.Padding = new Padding(15);
            this.tabListings.Controls.Add(this.dgvListings);
            this.tabListings.Controls.Add(this.btnMakeReservation);
            this.tabListings.Controls.Add(this.btnViewDetails);

            this.tabReservations.Text = "My Reservations";
            this.tabReservations.BackColor = System.Drawing.Color.White;
            this.tabReservations.Padding = new Padding(15);
            this.tabReservations.Controls.Add(this.dgvReservations);
            this.tabReservations.Controls.Add(this.btnCancelReservation);

            this.tabMyListings.Text = "My Listings";
            this.tabMyListings.BackColor = System.Drawing.Color.White;
            this.tabMyListings.Padding = new Padding(15);
            this.tabMyListings.Controls.Add(this.dgvMyListings);
            this.tabMyListings.Controls.Add(this.btnCreateListing);

            this.tabProfile.Text = "My Profile";
            this.tabProfile.BackColor = System.Drawing.Color.White;
            this.tabProfile.Padding = new Padding(15);

            // Listings DataGridView
            this.dgvListings.Dock = DockStyle.Top;
            this.dgvListings.Height = 400;
            this.dgvListings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvListings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvListings.MultiSelect = false;
            this.dgvListings.BackgroundColor = System.Drawing.Color.White;
            this.dgvListings.BorderStyle = BorderStyle.None;
            this.dgvListings.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvListings.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvListings.EnableHeadersVisualStyles = false;
            this.dgvListings.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dgvListings.RowHeadersVisible = false;
            this.dgvListings.RowTemplate.Height = 35;
            this.dgvListings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dgvListings.ReadOnly = true;
            this.dgvListings.AllowUserToResizeColumns = false;
            this.dgvListings.AllowUserToResizeRows = false;

            // Column Headers Style
            this.dgvListings.ColumnHeadersHeight = 40;
            this.dgvListings.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.dgvListings.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvListings.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.dgvListings.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Rows Style
            this.dgvListings.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvListings.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.dgvListings.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvListings.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Make Reservation Button
            this.btnMakeReservation.Text = "Make Reservation";
            this.btnMakeReservation.Location = new System.Drawing.Point(10, 420);
            this.btnMakeReservation.Size = new System.Drawing.Size(150, 40);
            this.btnMakeReservation.FlatStyle = FlatStyle.Flat;
            this.btnMakeReservation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnMakeReservation.ForeColor = System.Drawing.Color.White;
            this.btnMakeReservation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnMakeReservation.Cursor = Cursors.Hand;
            this.btnMakeReservation.Click += new EventHandler(BtnMakeReservation_Click);

            // View Details Button
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.Location = new System.Drawing.Point(170, 420);
            this.btnViewDetails.Size = new System.Drawing.Size(150, 40);
            this.btnViewDetails.FlatStyle = FlatStyle.Flat;
            this.btnViewDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnViewDetails.ForeColor = System.Drawing.Color.White;
            this.btnViewDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnViewDetails.Cursor = Cursors.Hand;
            this.btnViewDetails.Click += new EventHandler(BtnViewDetails_Click);

            // Reservations DataGridView
            this.dgvReservations.Dock = DockStyle.Top;
            this.dgvReservations.Height = 400;
            this.dgvReservations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservations.MultiSelect = false;
            this.dgvReservations.BackgroundColor = System.Drawing.Color.White;
            this.dgvReservations.BorderStyle = BorderStyle.None;
            this.dgvReservations.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvReservations.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvReservations.EnableHeadersVisualStyles = false;
            this.dgvReservations.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dgvReservations.RowHeadersVisible = false;
            this.dgvReservations.RowTemplate.Height = 35;
            this.dgvReservations.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dgvReservations.ReadOnly = true;
            this.dgvReservations.AllowUserToResizeColumns = false;
            this.dgvReservations.AllowUserToResizeRows = false;
            
            // Column Headers Style
            this.dgvReservations.ColumnHeadersHeight = 40;
            this.dgvReservations.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.dgvReservations.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvReservations.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.dgvReservations.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Rows Style
            this.dgvReservations.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvReservations.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.dgvReservations.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvReservations.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Cancel Reservation Button
            this.btnCancelReservation.Text = "Cancel Reservation";
            this.btnCancelReservation.Location = new System.Drawing.Point(10, 420);
            this.btnCancelReservation.Size = new System.Drawing.Size(150, 40);
            this.btnCancelReservation.FlatStyle = FlatStyle.Flat;
            this.btnCancelReservation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancelReservation.ForeColor = System.Drawing.Color.White;
            this.btnCancelReservation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancelReservation.Cursor = Cursors.Hand;
            this.btnCancelReservation.Click += new EventHandler(BtnCancelReservation_Click);

            // Create Listing Button
            this.btnCreateListing.Text = "Create Listing";
            this.btnCreateListing.Location = new System.Drawing.Point(10, 420);
            this.btnCreateListing.Size = new System.Drawing.Size(150, 40);
            this.btnCreateListing.FlatStyle = FlatStyle.Flat;
            this.btnCreateListing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCreateListing.ForeColor = System.Drawing.Color.White;
            this.btnCreateListing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCreateListing.Cursor = Cursors.Hand;
            this.btnCreateListing.Click += new EventHandler(BtnCreateListing_Click);

            // My Listings DataGridView
            this.dgvMyListings.Dock = DockStyle.Top;
            this.dgvMyListings.Height = 400;
            this.dgvMyListings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMyListings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMyListings.MultiSelect = false;
            this.dgvMyListings.BackgroundColor = System.Drawing.Color.White;
            this.dgvMyListings.BorderStyle = BorderStyle.None;
            this.dgvMyListings.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMyListings.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvMyListings.EnableHeadersVisualStyles = false;
            this.dgvMyListings.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dgvMyListings.RowHeadersVisible = false;
            this.dgvMyListings.RowTemplate.Height = 35;
            this.dgvMyListings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dgvMyListings.ReadOnly = true;
            this.dgvMyListings.AllowUserToResizeColumns = false;
            this.dgvMyListings.AllowUserToResizeRows = false;

            // Column Headers Style for My Listings
            this.dgvMyListings.ColumnHeadersHeight = 40;
            this.dgvMyListings.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.dgvMyListings.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvMyListings.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.dgvMyListings.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Rows Style for My Listings
            this.dgvMyListings.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvMyListings.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.dgvMyListings.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvMyListings.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Profil Paneli Modern Tasarım
            this.pnlProfileCard = new Panel();
            this.pnlProfileCard.Dock = DockStyle.Fill;
            this.pnlProfileCard.BackColor = System.Drawing.Color.WhiteSmoke;

            // Başlık
            this.lblProfileTitle = new Label();
            this.lblProfileTitle.Text = "My Profile";
            this.lblProfileTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblProfileTitle.ForeColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.lblProfileTitle.AutoSize = true;
            this.lblProfileTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProfileTitle.Dock = DockStyle.None;
            this.lblProfileTitle.Location = new Point(30, 30);
            this.lblProfileTitle.Padding = new Padding(0, 0, 0, 0);

            // TableLayoutPanel
            this.tblProfileLayout = new TableLayoutPanel();
            this.tblProfileLayout.Dock = DockStyle.None;
            this.tblProfileLayout.ColumnCount = 2;
            this.tblProfileLayout.RowCount = 6;
            this.tblProfileLayout.Width = 500;
            this.tblProfileLayout.Height = 260;
            this.tblProfileLayout.Location = new Point(300, 60); // Başlığın sağ altı
            this.tblProfileLayout.BackColor = System.Drawing.Color.Transparent;
            this.tblProfileLayout.ColumnStyles.Clear();
            this.tblProfileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.tblProfileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            this.tblProfileLayout.RowStyles.Clear();
            for (int i = 0; i < 6; i++)
                this.tblProfileLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            // Label ve textbox'lar
            this.lblName.Text = "Name";
            this.lblSurname.Text = "Surname";
            this.lblOldPassword.Text = "Old Password";
            this.lblNewPassword.Text = "New Password";
            this.lblNewPasswordRepeat.Text = "Repeat New Password";
            this.lblAbout.Text = "About";

            Label[] labels = { lblName, lblSurname, lblOldPassword, lblNewPassword, lblNewPasswordRepeat, lblAbout };
            foreach (var lbl in labels)
            {
                lbl.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
                lbl.ForeColor = System.Drawing.Color.FromArgb(33, 150, 243);
                lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                lbl.Dock = DockStyle.Fill;
            }

            TextBox[] textboxes = { txtName, txtSurname, txtOldPassword, txtNewPassword, txtNewPasswordRepeat, txtAbout };
            foreach (var tb in textboxes)
            {
                tb.Font = new System.Drawing.Font("Segoe UI", 11F);
                tb.BackColor = System.Drawing.Color.White;
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.ForeColor = System.Drawing.Color.Black;
                tb.Dock = DockStyle.Fill;
                tb.Margin = new Padding(5, 8, 10, 8);
            }
            this.txtAbout.Multiline = true;
            this.txtAbout.Height = 60;

            // TableLayoutPanel'e ekle
            this.tblProfileLayout.Controls.Clear();
            this.tblProfileLayout.Controls.Add(this.lblName, 0, 0);
            this.tblProfileLayout.Controls.Add(this.txtName, 1, 0);
            this.tblProfileLayout.Controls.Add(this.lblSurname, 0, 1);
            this.tblProfileLayout.Controls.Add(this.txtSurname, 1, 1);
            this.tblProfileLayout.Controls.Add(this.lblOldPassword, 0, 2);
            this.tblProfileLayout.Controls.Add(this.txtOldPassword, 1, 2);
            this.tblProfileLayout.Controls.Add(this.lblNewPassword, 0, 3);
            this.tblProfileLayout.Controls.Add(this.txtNewPassword, 1, 3);
            this.tblProfileLayout.Controls.Add(this.lblNewPasswordRepeat, 0, 4);
            this.tblProfileLayout.Controls.Add(this.txtNewPasswordRepeat, 1, 4);
            this.tblProfileLayout.Controls.Add(this.lblAbout, 0, 5);
            this.tblProfileLayout.Controls.Add(this.txtAbout, 1, 5);

            // Save Butonu
            this.btnSaveProfile.Text = "Save";
            this.btnSaveProfile.Dock = DockStyle.None;
            this.btnSaveProfile.Size = new System.Drawing.Size(180, 40);
            this.btnSaveProfile.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSaveProfile.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.btnSaveProfile.ForeColor = System.Drawing.Color.White;
            this.btnSaveProfile.FlatStyle = FlatStyle.Flat;
            this.btnSaveProfile.Anchor = AnchorStyles.Top;
            this.btnSaveProfile.Location = new System.Drawing.Point(
                this.tblProfileLayout.Location.X + (this.tblProfileLayout.Width - this.btnSaveProfile.Width) / 2,
                this.tblProfileLayout.Location.Y + this.tblProfileLayout.Height + 10
            );
            this.btnSaveProfile.Click += new EventHandler(BtnSaveProfile_Click);

            // Bilgi Label'ı
            this.lblProfileInfo.Dock = DockStyle.None;
            this.lblProfileInfo.Size = new System.Drawing.Size(500, 40);
            this.lblProfileInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblProfileInfo.ForeColor = System.Drawing.Color.Green;
            this.lblProfileInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProfileInfo.Location = new System.Drawing.Point(
                this.tblProfileLayout.Location.X,
                this.btnSaveProfile.Location.Y + this.btnSaveProfile.Height + 10
            );

            // Panelin içine ekle
            this.pnlProfileCard.Controls.Clear();
            this.pnlProfileCard.Controls.Add(this.lblProfileTitle);
            this.pnlProfileCard.Controls.Add(this.tblProfileLayout);
            this.pnlProfileCard.Controls.Add(this.btnSaveProfile);
            this.pnlProfileCard.Controls.Add(this.lblProfileInfo);

            // Tab'a ekle
            this.tabProfile.Controls.Clear();
            this.tabProfile.Controls.Add(this.pnlProfileCard);

            // Add controls to form
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnSwitchAccount);
            this.Controls.Add(this.btnLogout);
        }

        private void LoadListings()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            l.listingID,
                            CONCAT(u.name, ' ', u.surname) as ownerName,
                            l.listingTitle,
                            l.rentalPrice,
                            l.listingState,
                            l.listingDescription
                        FROM Listings l
                        INNER JOIN Users u ON l.userID = u.userID
                        WHERE l.userID != @CurrentUserID 
                        AND l.listingState = 1
                        ORDER BY l.listingID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CurrentUserID", _client.UserID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            dgvListings.DataSource = dt;

                            // Configure column headers
                            if (dgvListings.Columns.Count > 0)
                            {
                                dgvListings.Columns["listingID"].Visible = false;
                                dgvListings.Columns["ownerName"].HeaderText = "Listed By";
                                dgvListings.Columns["listingTitle"].HeaderText = "Title";
                                dgvListings.Columns["rentalPrice"].HeaderText = "Price (₺)";
                                dgvListings.Columns["listingState"].Visible = false;
                                dgvListings.Columns["listingDescription"].HeaderText = "Description";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading listings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReservations()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            r.reservationID,
                            l.listingTitle,
                            r.checkInDate,
                            r.checkOutDate,
                            CASE 
                                WHEN r.IsPaid = 1 THEN 'Paid'
                                ELSE 'Unpaid'
                            END as PaymentStatus,
                            CASE 
                                WHEN r.reservationState = 1 THEN 'Active'
                                ELSE 'Inactive'
                            END as ReservationStatus,
                            '' as SpecialRequests
                        FROM Reservations r
                        INNER JOIN Listings l ON r.listingId = l.listingId
                        WHERE r.userId = @UserID
                        ORDER BY r.checkInDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", _client.UserID);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvReservations.DataSource = dataTable;
                        
                        // Format the columns
                        if (dataTable.Rows.Count > 0)
                        {
                            dgvReservations.Columns["reservationID"].Visible = false;
                            dgvReservations.Columns["listingTitle"].HeaderText = "Listing";
                            dgvReservations.Columns["checkInDate"].HeaderText = "Check-in Date";
                            dgvReservations.Columns["checkOutDate"].HeaderText = "Check-out Date";
                            dgvReservations.Columns["PaymentStatus"].HeaderText = "Payment Status";
                            dgvReservations.Columns["ReservationStatus"].HeaderText = "Status";
                            dgvReservations.Columns["SpecialRequests"].HeaderText = "Special Requests";

                            // Status sütununun görünümünü özelleştir
                            foreach (DataGridViewRow row in dgvReservations.Rows)
                            {
                                if (row.Cells["ReservationStatus"].Value != null)
                                {
                                    if (row.Cells["ReservationStatus"].Value.ToString() == "Active")
                                    {
                                        row.Cells["ReservationStatus"].Style.ForeColor = System.Drawing.Color.Green;
                                        row.Cells["ReservationStatus"].Style.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                                    }
                                    else
                                    {
                                        row.Cells["ReservationStatus"].Style.ForeColor = System.Drawing.Color.Red;
                                        row.Cells["ReservationStatus"].Style.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                                    }
                                }

                                // Ödeme durumunu özelleştir
                                if (row.Cells["PaymentStatus"].Value != null)
                                {
                                    string paymentStatus = row.Cells["PaymentStatus"].Value.ToString();
                                    row.Cells["PaymentStatus"].Style.ForeColor = paymentStatus == "Paid" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                                    row.Cells["PaymentStatus"].Style.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reservations: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMyListings()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            CONCAT(u.name, ' ', u.surname) as ownerName,
                            l.listingTitle,
                            l.rentalPrice,
                            l.listingState,
                            l.listingDescription
                        FROM Listings l
                        INNER JOIN Users u ON l.userID = u.userID
                        WHERE l.userID = @CurrentUserID 
                        ORDER BY l.listingID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CurrentUserID", _client.UserID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            dgvMyListings.DataSource = dt;

                            // Configure column headers
                            if (dgvMyListings.Columns.Count > 0)
                            {
                                dgvMyListings.Columns["ownerName"].HeaderText = "Listed By";
                                dgvMyListings.Columns["listingTitle"].HeaderText = "Title";
                                dgvMyListings.Columns["rentalPrice"].HeaderText = "Price (₺)";
                                dgvMyListings.Columns["listingState"].Visible = false;
                                dgvMyListings.Columns["listingDescription"].HeaderText = "Description";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading my listings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMakeReservation_Click(object sender, EventArgs e)
        {
            if (dgvListings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a listing first.", "Make Reservation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var reservationForm = new ReservationForm(_client, dgvListings.SelectedRows[0]);
            if (reservationForm.ShowDialog() == DialogResult.OK)
            {
                LoadReservations(); // Refresh the reservations list
            }
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvListings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a listing first.", "View Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Implement listing details view
            // Show a dialog with detailed information about the selected listing
        }

        private void BtnCancelReservation_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reservation first.", "Cancel Reservation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["reservationID"].Value);
            var listingTitle = dgvReservations.SelectedRows[0].Cells["listingTitle"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to cancel your reservation for \"{listingTitle}\"?", 
                "Cancel Reservation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                    {
                        connection.Open();
                        string query = @"
                            UPDATE Reservations 
                            SET reservationState = 0,
                                CancellationReason = 'Cancelled by client'
                            WHERE reservationID = @ReservationID AND userId = @UserID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ReservationID", reservationId);
                            command.Parameters.AddWithValue("@UserID", _client.UserID);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Reservation cancelled successfully!", "Cancel Reservation",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadReservations(); // Refresh the reservations list
                            }
                            else
                            {
                                MessageBox.Show("Could not cancel the reservation. Please try again.", 
                                    "Cancel Reservation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error cancelling reservation: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCreateListing_Click(object sender, EventArgs e)
        {
            var createListingForm = new CreateListingForm(_client);
            if (createListingForm.ShowDialog() == DialogResult.OK)
            {
                LoadMyListings(); // Refresh the listings after creating a new one
            }
        }

        private void BtnSwitchAccount_Click(object sender, EventArgs e)
        {
            // Login formunu göster, bu formu gizle
            this.Hide();
            var loginForm = new LoginForm();
            var result = loginForm.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Cancel)
            {
                this.Close();
            }
            else
            {
                this.Show();
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkış yapmak istediğinize emin misiniz?", "Çıkış Yap",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void BtnSaveProfile_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string newPasswordRepeat = txtNewPasswordRepeat.Text;
            string about = txtAbout.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
            {
                MessageBox.Show("Ad ve soyad boş olamaz.", "Profil Güncelle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Şifre değiştirme isteği var mı?
            bool changePassword = !string.IsNullOrEmpty(oldPassword) || !string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(newPasswordRepeat);
            if (changePassword)
            {
                if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(newPasswordRepeat))
                {
                    MessageBox.Show("Şifre değiştirmek için tüm şifre alanlarını doldurun.", "Profil Güncelle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (newPassword != newPasswordRepeat)
                {
                    MessageBox.Show("Yeni şifreler eşleşmiyor.", "Profil Güncelle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Eski şifre doğru mu kontrol et
                if (oldPassword != _client.Password)
                {
                    MessageBox.Show("Mevcut şifre yanlış.", "Profil Güncelle", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Veritabanında güncelle
            try
            {
                using (var connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Users SET Name = @Name, Surname = @Surname, About = @About";
                    if (changePassword)
                        query += ", Password = @Password";
                    query += " WHERE UserID = @UserID";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Surname", surname);
                        command.Parameters.AddWithValue("@About", about);
                        if (changePassword)
                            command.Parameters.AddWithValue("@Password", newPassword);
                        command.Parameters.AddWithValue("@UserID", _client.UserID);
                        command.ExecuteNonQuery();
                    }
                }
                // Local client objesini güncelle
                _client.Name = name;
                _client.Surname = surname;
                _client.About = about;
                if (changePassword)
                    _client.Password = newPassword;
                lblProfileInfo.Text = $"Profil güncellendi!\nAd: {name} {surname}\nHakkımda: {about}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Profil güncellenirken hata oluştu: {ex.Message}", "Profil Güncelle", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 