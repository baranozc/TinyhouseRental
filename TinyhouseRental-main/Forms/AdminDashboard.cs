using System;
using System.Windows.Forms;
using MyProject.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace MyProject.Forms
{
    public partial class AdminDashboard : Form
    {
        private Admin _admin;
        private TabControl tabControl;
        private TabPage tabUsers;
        private TabPage tabListings;
        private TabPage tabReservations;
        private DataGridView dgvUsers;
        private DataGridView dgvListings;
        private DataGridView dgvReservations;
        private Button btnFreezeUser;
        private Button btnUnfreezeUser;
        private Button btnActivateListing;
        private Button btnDeactivateListing;
        private Button btnApproveReservation;
        private Button btnCancelReservation;
        private Button btnLogout;
        private Button btnSwitchAccount;
        private Button btnMarkAsPaid;
        private Button btnMarkAsUnpaid;
        private Panel pnlTopBar;

        public AdminDashboard(Admin admin)
        {
            _admin = admin;
            InitializeComponent();
            LoadUsers();
            LoadListings();
            LoadReservations();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabUsers = new TabPage();
            this.tabListings = new TabPage();
            this.tabReservations = new TabPage();
            this.dgvUsers = new DataGridView();
            this.dgvListings = new DataGridView();
            this.dgvReservations = new DataGridView();
            this.btnFreezeUser = new Button();
            this.btnUnfreezeUser = new Button();
            this.btnActivateListing = new Button();
            this.btnDeactivateListing = new Button();
            this.btnApproveReservation = new Button();
            this.btnCancelReservation = new Button();
            this.btnLogout = new Button();
            this.btnSwitchAccount = new Button();
            this.btnMarkAsPaid = new Button();
            this.btnMarkAsUnpaid = new Button();
            this.pnlTopBar = new Panel();

            // Form
            this.Text = "Tiny House Rental - Admin Dashboard";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            // Top Bar
            this.pnlTopBar.Size = new System.Drawing.Size(this.Width, 55);
            this.pnlTopBar.Dock = DockStyle.Top;
            this.pnlTopBar.BackColor = Color.White;
            this.pnlTopBar.BorderStyle = BorderStyle.None;
            this.pnlTopBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Logout Button
            this.btnLogout.Text = "Çıkış Yap";
            this.btnLogout.Size = new System.Drawing.Size(100, 35);
            this.btnLogout.Location = new System.Drawing.Point(this.pnlTopBar.Width - 125, 10);
            this.btnLogout.FlatStyle = FlatStyle.Flat;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Cursor = Cursors.Hand;
            this.btnLogout.Click += BtnLogout_Click;
            this.btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Hesap Değiştir Butonu
            this.btnSwitchAccount.Text = "Hesap Değiştir";
            this.btnSwitchAccount.Size = new System.Drawing.Size(120, 35);
            this.btnSwitchAccount.Location = new System.Drawing.Point(this.pnlTopBar.Width - 255, 10);
            this.btnSwitchAccount.FlatStyle = FlatStyle.Flat;
            this.btnSwitchAccount.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.btnSwitchAccount.ForeColor = System.Drawing.Color.White;
            this.btnSwitchAccount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSwitchAccount.Cursor = Cursors.Hand;
            this.btnSwitchAccount.Click += BtnSwitchAccount_Click;
            this.btnSwitchAccount.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Panel'e butonları ekle
            this.pnlTopBar.Controls.Add(this.btnSwitchAccount);
            this.pnlTopBar.Controls.Add(this.btnLogout);

            // Tab Control
            this.tabControl.Location = new System.Drawing.Point(0, this.pnlTopBar.Height);
            this.tabControl.Size = new System.Drawing.Size(this.Width, this.Height - this.pnlTopBar.Height);
            this.tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tabControl.Padding = new System.Drawing.Point(10, 5);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.tabControl.Controls.AddRange(new Control[] {
                this.tabUsers,
                this.tabListings,
                this.tabReservations
            });

            // Users Tab
            this.tabUsers.Text = "User Management";
            this.tabUsers.BackColor = System.Drawing.Color.White;
            this.tabUsers.Padding = new Padding(15);
            this.tabUsers.Controls.Add(this.dgvUsers);
            this.tabUsers.Controls.Add(this.btnFreezeUser);
            this.tabUsers.Controls.Add(this.btnUnfreezeUser);

            // Users DataGridView
            this.dgvUsers.Dock = DockStyle.Top;
            this.dgvUsers.Height = 500;
            this.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = BorderStyle.None;
            this.dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.GridColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowTemplate.Height = 35;
            this.dgvUsers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.AllowUserToResizeColumns = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.ColumnHeadersHeight = 40;
            this.dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvUsers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.dgvUsers.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            this.dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.dgvUsers.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.dgvUsers.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvUsers.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            this.dgvUsers.CellFormatting += DgvUsers_CellFormatting;
            this.dgvUsers.DataError += (s, e) => { e.ThrowException = false; };

            // Freeze User Button
            this.btnFreezeUser.Text = "Freeze User";
            this.btnFreezeUser.Location = new System.Drawing.Point(170, 520);
            this.btnFreezeUser.Size = new System.Drawing.Size(150, 35);
            this.btnFreezeUser.FlatStyle = FlatStyle.Flat;
            this.btnFreezeUser.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnFreezeUser.ForeColor = System.Drawing.Color.White;
            this.btnFreezeUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFreezeUser.Cursor = Cursors.Hand;
            this.btnFreezeUser.Click += new EventHandler(BtnFreezeUser_Click);

            // Unfreeze User Button
            this.btnUnfreezeUser.Text = "Unfreeze User";
            this.btnUnfreezeUser.Location = new System.Drawing.Point(10, 520);
            this.btnUnfreezeUser.Size = new System.Drawing.Size(150, 35);
            this.btnUnfreezeUser.FlatStyle = FlatStyle.Flat;
            this.btnUnfreezeUser.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnUnfreezeUser.ForeColor = System.Drawing.Color.White;
            this.btnUnfreezeUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUnfreezeUser.Cursor = Cursors.Hand;
            this.btnUnfreezeUser.Click += new EventHandler(BtnUnfreezeUser_Click);

            // Listings Tab
            this.tabListings.Text = "Listing Management";
            this.tabListings.BackColor = System.Drawing.Color.White;
            this.tabListings.Padding = new Padding(15);
            this.tabListings.Controls.Add(this.dgvListings);
            this.tabListings.Controls.Add(this.btnActivateListing);
            this.tabListings.Controls.Add(this.btnDeactivateListing);

            // Listings DataGridView
            this.dgvListings.Dock = DockStyle.Top;
            this.dgvListings.Height = 500;
            this.dgvListings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvListings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvListings.MultiSelect = false;
            this.dgvListings.BackgroundColor = System.Drawing.Color.White;
            this.dgvListings.BorderStyle = BorderStyle.None;
            this.dgvListings.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvListings.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvListings.EnableHeadersVisualStyles = false;
            this.dgvListings.GridColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.dgvListings.RowHeadersVisible = false;
            this.dgvListings.RowTemplate.Height = 35;
            this.dgvListings.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvListings.ReadOnly = true;
            this.dgvListings.AllowUserToResizeColumns = false;
            this.dgvListings.AllowUserToResizeRows = false;
            this.dgvListings.ColumnHeadersHeight = 40;
            this.dgvListings.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.dgvListings.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvListings.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.dgvListings.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            this.dgvListings.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.dgvListings.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.dgvListings.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvListings.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            this.dgvListings.CellFormatting += DgvListings_CellFormatting;
            this.dgvListings.DataError += (s, e) => { e.ThrowException = false; };

            // Add a button column for showing image in Listings
            DataGridViewButtonColumn viewListingImageButtonColumn = new DataGridViewButtonColumn();
            viewListingImageButtonColumn.HeaderText = "Görsel";
            viewListingImageButtonColumn.Text = "Göster";
            viewListingImageButtonColumn.UseColumnTextForButtonValue = true;
            viewListingImageButtonColumn.Name = "viewListingImageColumn";
            this.dgvListings.Columns.Add(viewListingImageButtonColumn);

            // Handle button clicks in dgvListings
            this.dgvListings.CellContentClick += new DataGridViewCellEventHandler(DgvListings_CellContentClick);

            // Activate Listing Button
            this.btnActivateListing.Text = "Activate Listing";
            this.btnActivateListing.Location = new System.Drawing.Point(10, 520);
            this.btnActivateListing.Size = new System.Drawing.Size(150, 35);
            this.btnActivateListing.FlatStyle = FlatStyle.Flat;
            this.btnActivateListing.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnActivateListing.ForeColor = System.Drawing.Color.White;
            this.btnActivateListing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnActivateListing.Cursor = Cursors.Hand;
            this.btnActivateListing.Click += new EventHandler(BtnActivateListing_Click);

            // Deactivate Listing Button
            this.btnDeactivateListing.Text = "Deactivate Listing";
            this.btnDeactivateListing.Location = new System.Drawing.Point(170, 520);
            this.btnDeactivateListing.Size = new System.Drawing.Size(150, 35);
            this.btnDeactivateListing.FlatStyle = FlatStyle.Flat;
            this.btnDeactivateListing.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnDeactivateListing.ForeColor = System.Drawing.Color.White;
            this.btnDeactivateListing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeactivateListing.Cursor = Cursors.Hand;
            this.btnDeactivateListing.Click += new EventHandler(BtnDeactivateListing_Click);

            // Reservations Tab
            this.tabReservations.Text = "Reservation Management";
            this.tabReservations.BackColor = System.Drawing.Color.White;
            this.tabReservations.Padding = new Padding(15);
            this.tabReservations.Controls.Add(this.dgvReservations);
            this.tabReservations.Controls.Add(this.btnApproveReservation);
            this.tabReservations.Controls.Add(this.btnCancelReservation);
            this.tabReservations.Controls.Add(this.btnMarkAsPaid);
            this.tabReservations.Controls.Add(this.btnMarkAsUnpaid);

            // Reservations DataGridView
            this.dgvReservations.Dock = DockStyle.Top;
            this.dgvReservations.Height = 500;
            this.dgvReservations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservations.MultiSelect = false;
            this.dgvReservations.BackgroundColor = System.Drawing.Color.White;
            this.dgvReservations.BorderStyle = BorderStyle.None;
            this.dgvReservations.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvReservations.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvReservations.EnableHeadersVisualStyles = false;
            this.dgvReservations.GridColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.dgvReservations.RowHeadersVisible = false;
            this.dgvReservations.RowTemplate.Height = 35;
            this.dgvReservations.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvReservations.ReadOnly = false;
            this.dgvReservations.AllowUserToResizeColumns = false;
            this.dgvReservations.AllowUserToResizeRows = false;
            this.dgvReservations.ColumnHeadersHeight = 40;
            this.dgvReservations.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.dgvReservations.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvReservations.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.dgvReservations.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            this.dgvReservations.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.dgvReservations.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.dgvReservations.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvReservations.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            this.dgvReservations.CellFormatting += DgvReservations_CellFormatting;
            this.dgvReservations.DataError += (s, e) => { e.ThrowException = false; };
            this.dgvReservations.CellValueChanged += DgvReservations_CellValueChanged;
            this.dgvReservations.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvReservations.IsCurrentCellDirty)
                    dgvReservations.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            // Add a button column for showing image in Reservations
            DataGridViewButtonColumn viewReservationImageButtonColumn = new DataGridViewButtonColumn();
            viewReservationImageButtonColumn.HeaderText = "Görsel";
            viewReservationImageButtonColumn.Text = "Göster";
            viewReservationImageButtonColumn.UseColumnTextForButtonValue = true;
            viewReservationImageButtonColumn.Name = "viewReservationImageColumn";
            this.dgvReservations.Columns.Add(viewReservationImageButtonColumn);

            // Handle button clicks in dgvReservations
            this.dgvReservations.CellContentClick += new DataGridViewCellEventHandler(DgvReservations_CellContentClick);

            // Approve Reservation Button
            this.btnApproveReservation.Text = "Approve Reservation";
            this.btnApproveReservation.Location = new System.Drawing.Point(10, 520);
            this.btnApproveReservation.Size = new System.Drawing.Size(180, 35);
            this.btnApproveReservation.FlatStyle = FlatStyle.Flat;
            this.btnApproveReservation.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnApproveReservation.ForeColor = System.Drawing.Color.White;
            this.btnApproveReservation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnApproveReservation.Cursor = Cursors.Hand;
            this.btnApproveReservation.Click += new EventHandler(BtnApproveReservation_Click);

            // Cancel Reservation Button
            this.btnCancelReservation.Text = "Cancel Reservation";
            this.btnCancelReservation.Location = new System.Drawing.Point(200, 520);
            this.btnCancelReservation.Size = new System.Drawing.Size(180, 35);
            this.btnCancelReservation.FlatStyle = FlatStyle.Flat;
            this.btnCancelReservation.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnCancelReservation.ForeColor = System.Drawing.Color.White;
            this.btnCancelReservation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelReservation.Cursor = Cursors.Hand;
            this.btnCancelReservation.Click += new EventHandler(BtnCancelReservation_Click);

            // Mark as Paid Button
            this.btnMarkAsPaid.Text = "Mark as Paid";
            this.btnMarkAsPaid.Location = new System.Drawing.Point(400, 520);
            this.btnMarkAsPaid.Size = new System.Drawing.Size(150, 35);
            this.btnMarkAsPaid.FlatStyle = FlatStyle.Flat;
            this.btnMarkAsPaid.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnMarkAsPaid.ForeColor = System.Drawing.Color.White;
            this.btnMarkAsPaid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMarkAsPaid.Cursor = Cursors.Hand;
            this.btnMarkAsPaid.Click += new EventHandler(BtnMarkAsPaid_Click);

            // Mark as Unpaid Button
            this.btnMarkAsUnpaid.Text = "Mark as Unpaid";
            this.btnMarkAsUnpaid.Location = new System.Drawing.Point(560, 520);
            this.btnMarkAsUnpaid.Size = new System.Drawing.Size(150, 35);
            this.btnMarkAsUnpaid.FlatStyle = FlatStyle.Flat;
            this.btnMarkAsUnpaid.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnMarkAsUnpaid.ForeColor = System.Drawing.Color.White;
            this.btnMarkAsUnpaid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMarkAsUnpaid.Cursor = Cursors.Hand;
            this.btnMarkAsUnpaid.Click += new EventHandler(BtnMarkAsUnpaid_Click);

            // Add controls to form
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.tabControl);
        }

        private void LoadUsers()
        {
            try
            {
                using (var connection = new System.Data.SqlClient.SqlConnection(MyProject.Config.DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            userID AS [UserID],
                            email AS [Email],
                            password AS [Password],
                            name AS [Name],
                            surname AS [Surname],
                            'Client' AS [Type],
                            userState AS [State]
                        FROM Users
                        WHERE userTypeId = 1";

                    using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                    using (var adapter = new System.Data.SqlClient.SqlDataAdapter(command))
                    {
                        var dt = new System.Data.DataTable();
                        adapter.Fill(dt);
                        dgvUsers.DataSource = dt;
                        // State sütununu kaldırıp yerine string tipinde bir sütun ekle
                        if (dgvUsers.Columns["State"] != null)
                        {
                            int stateIndex = dgvUsers.Columns["State"].Index;
                            dgvUsers.Columns.Remove("State");
                            var col = new DataGridViewTextBoxColumn();
                            col.Name = "State";
                            col.HeaderText = "State";
                            col.DataPropertyName = "State";
                            col.ReadOnly = true;
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            dgvUsers.Columns.Insert(stateIndex, col);
                        }
                        // Her satır için string değer ata
                        foreach (DataGridViewRow row in dgvUsers.Rows)
                        {
                            if (row.Cells["State"].Value != null)
                            {
                                string val = row.Cells["State"].Value.ToString().ToLower();
                                bool isActive = val == "true" || val == "1";
                                row.Cells["State"].Value = isActive ? "✔️ Active" : "❌ Inactive";
                                row.Cells["State"].Style.ForeColor = isActive
                                    ? System.Drawing.Color.FromArgb(40, 167, 69)
                                    : System.Drawing.Color.FromArgb(220, 53, 69);
                                row.Cells["State"].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadListings()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(MyProject.Config.DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            l.listingID,
                            CONCAT(u.name, ' ', u.surname) as ownerName,
                            l.listingTitle,
                            l.rentalPrice,
                            l.listingState,
                            l.ImageUrl -- ImageUrl sütununu ekledik
                        FROM Listings l
                        INNER JOIN Users u ON l.userID = u.userID
                        ORDER BY l.listingID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dgvListings.DataSource = dt;

                            // Configure column headers
                            if (dgvListings.Columns.Contains("listingID")) dgvListings.Columns["listingID"].Visible = false;
                            if (dgvListings.Columns.Contains("ownerName")) dgvListings.Columns["ownerName"].HeaderText = "Owner";
                            if (dgvListings.Columns.Contains("listingTitle")) dgvListings.Columns["listingTitle"].HeaderText = "Title";
                            if (dgvListings.Columns.Contains("rentalPrice")) dgvListings.Columns["rentalPrice"].HeaderText = "Price (₺)";
                            if (dgvListings.Columns.Contains("listingState")) dgvListings.Columns["listingState"].HeaderText = "State";

                            // ImageUrl sütununu gizle
                            if (dgvListings.Columns.Contains("ImageUrl"))
                            {
                                dgvListings.Columns["ImageUrl"].Visible = false;
                            }
                             // Görsel butonu sütununu en sağa taşı (varsa)
                            if (dgvListings.Columns.Contains("viewListingImageColumn"))
                            {
                                dgvListings.Columns["viewListingImageColumn"].DisplayIndex = dgvListings.Columns.Count - 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading listings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReservations()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(MyProject.Config.DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            r.reservationID,
                            CONCAT(u.name, ' ', u.surname) as userName,
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
                            l.ImageUrl -- ImageUrl sütununu ekledik
                        FROM Reservations r
                        INNER JOIN Users u ON r.userId = u.userID
                        INNER JOIN Listings l ON r.listingId = l.listingId -- Listings tablosunu JOIN ettik
                        ORDER BY r.checkInDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvReservations.DataSource = dataTable;

                        // Format the columns
                        if (dgvReservations.Columns.Contains("reservationID")) dgvReservations.Columns["reservationID"].Visible = false;
                        if (dgvReservations.Columns.Contains("userName")) dgvReservations.Columns["userName"].HeaderText = "User";
                        if (dgvReservations.Columns.Contains("listingTitle")) dgvReservations.Columns["listingTitle"].HeaderText = "Listing";
                        if (dgvReservations.Columns.Contains("checkInDate")) dgvReservations.Columns["checkInDate"].HeaderText = "Check-in Date";
                        if (dgvReservations.Columns.Contains("checkOutDate")) dgvReservations.Columns["checkOutDate"].HeaderText = "Check-out Date";
                        if (dgvReservations.Columns.Contains("PaymentStatus")) dgvReservations.Columns["PaymentStatus"].HeaderText = "Payment Status";
                        if (dgvReservations.Columns.Contains("ReservationStatus")) dgvReservations.Columns["ReservationStatus"].HeaderText = "Status";

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
                         // ImageUrl sütununu gizle
                        if (dgvReservations.Columns.Contains("ImageUrl"))
                        {
                            dgvReservations.Columns["ImageUrl"].Visible = false;
                        }
                         // Görsel butonu sütununu en sağa taşı (varsa)
                            if (dgvReservations.Columns.Contains("viewReservationImageColumn"))
                            {
                                dgvReservations.Columns["viewReservationImageColumn"].DisplayIndex = dgvReservations.Columns.Count - 1;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reservations: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFreezeUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.", "Freeze User",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
            var response = _admin.FreezerAccount(userId, false); // false: dondur

            if (response.Success)
            {
                MessageBox.Show(response.Message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers(); // Refresh the users list
            }
            else
            {
                MessageBox.Show(response.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUnfreezeUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.", "Unfreeze User",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Are you sure you want to unfreeze this user?", "Unfreeze User",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
            var response = _admin.FreezerAccount(userId, true); // true: aktif yap

            if (response.Success)
            {
                MessageBox.Show(response.Message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers(); // Refresh the users list
            }
            else
            {
                MessageBox.Show(response.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnActivateListing_Click(object sender, EventArgs e)
        {
            if (dgvListings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a listing first.", "Activate Listing",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int listingId = Convert.ToInt32(dgvListings.SelectedRows[0].Cells["ListingID"].Value);
            var response = _admin.ActivateListing(listingId);

            if (response.Success)
            {
                MessageBox.Show(response.Message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListings(); // Refresh the listings list
            }
            else
            {
                MessageBox.Show(response.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeactivateListing_Click(object sender, EventArgs e)
        {
            if (dgvListings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a listing first.", "Deactivate Listing",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Are you sure you want to deactivate this listing?", "Deactivate Listing",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            int listingId = Convert.ToInt32(dgvListings.SelectedRows[0].Cells["ListingID"].Value);
            var response = _admin.DeactivateListing(listingId);

            if (response.Success)
            {
                MessageBox.Show(response.Message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListings(); // Refresh the listings list
            }
            else
            {
                MessageBox.Show(response.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnApproveReservation_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reservation first.", "Approve Reservation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["ReservationID"].Value);
            var response = _admin.ApproveReservation(reservationId);

            if (response.Success)
            {
                MessageBox.Show(response.Message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations(); // Refresh the reservations list
            }
            else
            {
                MessageBox.Show(response.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelReservation_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reservation first.", "Cancel Reservation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to cancel this reservation?", "Cancel Reservation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["ReservationID"].Value);
                var response = _admin.CancelReservation(reservationId);

                if (response.Success)
                {
                    MessageBox.Show(response.Message, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReservations(); // Refresh the reservations list
                }
                else
                {
                    MessageBox.Show(response.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSwitchAccount_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Hesap değiştirmek istediğinize emin misiniz?", "Hesap Değiştir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
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
        }

        private void DgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsers.Columns[e.ColumnIndex].HeaderText == "State" && e.Value != null)
            {
                bool isActive = false;
                if (e.Value is bool)
                    isActive = (bool)e.Value;
                else if (e.Value is string)
                    isActive = e.Value.ToString().ToLower() == "true";

                if (isActive)
                {
                    e.Value = "✔️ Active";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
                }
                else
                {
                    e.Value = "❌ Inactive";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
                }
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void DgvListings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvListings.Columns[e.ColumnIndex].HeaderText == "State" && e.Value != null)
            {
                bool isActive = false;
                if (e.Value is bool)
                    isActive = (bool)e.Value;
                else if (e.Value is string)
                    isActive = e.Value.ToString().ToLower() == "true";

                if (isActive)
                {
                    e.Value = "✔️ Active";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
                }
                else
                {
                    e.Value = "❌ Inactive";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
                }
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void DgvReservations_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReservations.Columns[e.ColumnIndex].HeaderText == "reservationState" && e.Value != null)
            {
                bool isActive = false;
                if (e.Value is bool)
                    isActive = (bool)e.Value;
                else if (e.Value is string)
                    isActive = e.Value.ToString().ToLower() == "true";

                if (isActive)
                {
                    e.Value = "✔️ Active";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
                }
                else
                {
                    e.Value = "❌ Inactive";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
                }
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvReservations.Columns[e.ColumnIndex].HeaderText == "IsPaid" && e.Value != null)
            {
                bool isPaid = false;
                if (e.Value is bool)
                    isPaid = (bool)e.Value;
                else if (e.Value is string)
                    isPaid = e.Value.ToString().ToLower() == "true";

                if (isPaid)
                {
                    e.Value = "✔️ Paid";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
                }
                else
                {
                    e.Value = "❌ Unpaid";
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
                }
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void DgvReservations_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = dgvReservations;
            var row = grid.Rows[e.RowIndex];
            int reservationId = Convert.ToInt32(row.Cells["reservationID"].Value);
            if (grid.Columns[e.ColumnIndex].Name == "reservationState" || grid.Columns[e.ColumnIndex].Name == "IsPaid")
            {
                bool newValue = Convert.ToBoolean(row.Cells[e.ColumnIndex].Value);
                string columnName = grid.Columns[e.ColumnIndex].Name;
                try
                {
                    using (var connection = new System.Data.SqlClient.SqlConnection(MyProject.Config.DatabaseConfig.ConnectionString))
                    {
                        connection.Open();
                        string query = $"UPDATE Reservations SET {columnName} = @value WHERE reservationID = @id";
                        using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@value", newValue);
                            command.Parameters.AddWithValue("@id", reservationId);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnMarkAsPaid_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reservation first.", "Mark as Paid",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["reservationID"].Value);
            try
            {
                using (var connection = new System.Data.SqlClient.SqlConnection(MyProject.Config.DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Reservations SET IsPaid = 1 WHERE reservationID = @id";
                    using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", reservationId);
                        command.ExecuteNonQuery();
                    }
                }
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating IsPaid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarkAsUnpaid_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reservation first.", "Mark as Unpaid",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Are you sure you want to mark this reservation as unpaid?", "Mark as Unpaid",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["reservationID"].Value);
            try
            {
                using (var connection = new System.Data.SqlClient.SqlConnection(MyProject.Config.DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Reservations SET IsPaid = 0 WHERE reservationID = @id";
                    using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", reservationId);
                        command.ExecuteNonQuery();
                    }
                }
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating IsPaid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış Yap",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Handle button clicks in dgvListings (Admin)
        private void DgvListings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             // Check if the clicked cell is in the button column and it's not the header row
            if (e.ColumnIndex >= 0 && dgvListings.Columns[e.ColumnIndex].Name == "viewListingImageColumn" && e.RowIndex >= 0)
            {
                // Get the ImageUrl from the selected row
                DataGridViewRow selectedRow = dgvListings.Rows[e.RowIndex];
                string imageUrlsString = selectedRow.Cells["ImageUrl"].Value?.ToString();

                if (!string.IsNullOrEmpty(imageUrlsString))
                {
                    // Split the comma-separated string into individual image URLs
                    List<string> imageUrls = imageUrlsString.Split(',').ToList();

                    // Construct the absolute paths to the image files
                    List<string> absoluteImagePaths = new List<string>();
                    foreach (string imageUrl in imageUrls)
                    {
                        string trimmedImageUrl = imageUrl.Trim();
                        if (!string.IsNullOrEmpty(trimmedImageUrl))
                        {
                            absoluteImagePaths.Add(Path.Combine(Application.StartupPath, trimmedImageUrl));
                        }
                    }

                    if (absoluteImagePaths.Count > 0)
                    {
                        // Show the images in the image viewer form
                        ImageViewerForm imageViewer = new ImageViewerForm(absoluteImagePaths);
                        imageViewer.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Listing için geçerli görsel yolu bulunamadı.", "Görsel Yok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Bu listing için görsel bulunamadı.", "Görsel Yok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Handle button clicks in dgvReservations (Admin)
        private void DgvReservations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             // Check if the clicked cell is in the button column and it's not the header row
            if (e.ColumnIndex >= 0 && dgvReservations.Columns[e.ColumnIndex].Name == "viewReservationImageColumn" && e.RowIndex >= 0)
            {
                // Get the ImageUrl from the selected row
                DataGridViewRow selectedRow = dgvReservations.Rows[e.RowIndex];
                string imageUrlsString = selectedRow.Cells["ImageUrl"].Value?.ToString();

                if (!string.IsNullOrEmpty(imageUrlsString))
                {
                    // Split the comma-separated string into individual image URLs
                    List<string> imageUrls = imageUrlsString.Split(',').ToList();

                    // Construct the absolute paths to the image files
                    List<string> absoluteImagePaths = new List<string>();
                    foreach (string imageUrl in imageUrls)
                    {
                        string trimmedImageUrl = imageUrl.Trim();
                        if (!string.IsNullOrEmpty(trimmedImageUrl))
                        {
                            absoluteImagePaths.Add(Path.Combine(Application.StartupPath, trimmedImageUrl));
                        }
                    }

                    if (absoluteImagePaths.Count > 0)
                    {
                        // Show the images in the image viewer form
                        ImageViewerForm imageViewer = new ImageViewerForm(absoluteImagePaths);
                        imageViewer.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Listing için geçerli görsel yolu bulunamadı.", "Görsel Yok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Bu listing için görsel bulunamadı.", "Görsel Yok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
} 