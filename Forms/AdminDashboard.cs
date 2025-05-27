using System;
using System.Windows.Forms;

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
        private Button btnActivateListing;
        private Button btnDeactivateListing;
        private Button btnApproveReservation;
        private Button btnCancelReservation;

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
            this.btnActivateListing = new Button();
            this.btnDeactivateListing = new Button();
            this.btnApproveReservation = new Button();
            this.btnCancelReservation = new Button();

            // Form
            this.Text = "Tiny House Rental - Admin Dashboard";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tab Control
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Controls.AddRange(new Control[] {
                this.tabUsers,
                this.tabListings,
                this.tabReservations
            });

            // Users Tab
            this.tabUsers.Text = "User Management";
            this.tabUsers.Controls.Add(this.dgvUsers);
            this.tabUsers.Controls.Add(this.btnFreezeUser);

            // Users DataGridView
            this.dgvUsers.Dock = DockStyle.Top;
            this.dgvUsers.Height = 500;
            this.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.MultiSelect = false;

            // Freeze User Button
            this.btnFreezeUser.Text = "Freeze/Unfreeze User";
            this.btnFreezeUser.Location = new System.Drawing.Point(10, 520);
            this.btnFreezeUser.Size = new System.Drawing.Size(150, 30);
            this.btnFreezeUser.Click += new EventHandler(BtnFreezeUser_Click);

            // Listings Tab
            this.tabListings.Text = "Listing Management";
            this.tabListings.Controls.Add(this.dgvListings);
            this.tabListings.Controls.Add(this.btnActivateListing);
            this.tabListings.Controls.Add(this.btnDeactivateListing);

            // Listings DataGridView
            this.dgvListings.Dock = DockStyle.Top;
            this.dgvListings.Height = 500;
            this.dgvListings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvListings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvListings.MultiSelect = false;

            // Activate Listing Button
            this.btnActivateListing.Text = "Activate Listing";
            this.btnActivateListing.Location = new System.Drawing.Point(10, 520);
            this.btnActivateListing.Size = new System.Drawing.Size(120, 30);
            this.btnActivateListing.Click += new EventHandler(BtnActivateListing_Click);

            // Deactivate Listing Button
            this.btnDeactivateListing.Text = "Deactivate Listing";
            this.btnDeactivateListing.Location = new System.Drawing.Point(140, 520);
            this.btnDeactivateListing.Size = new System.Drawing.Size(120, 30);
            this.btnDeactivateListing.Click += new EventHandler(BtnDeactivateListing_Click);

            // Reservations Tab
            this.tabReservations.Text = "Reservation Management";
            this.tabReservations.Controls.Add(this.dgvReservations);
            this.tabReservations.Controls.Add(this.btnApproveReservation);
            this.tabReservations.Controls.Add(this.btnCancelReservation);

            // Reservations DataGridView
            this.dgvReservations.Dock = DockStyle.Top;
            this.dgvReservations.Height = 500;
            this.dgvReservations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservations.MultiSelect = false;

            // Approve Reservation Button
            this.btnApproveReservation.Text = "Approve Reservation";
            this.btnApproveReservation.Location = new System.Drawing.Point(10, 520);
            this.btnApproveReservation.Size = new System.Drawing.Size(150, 30);
            this.btnApproveReservation.Click += new EventHandler(BtnApproveReservation_Click);

            // Cancel Reservation Button
            this.btnCancelReservation.Text = "Cancel Reservation";
            this.btnCancelReservation.Location = new System.Drawing.Point(170, 520);
            this.btnCancelReservation.Size = new System.Drawing.Size(150, 30);
            this.btnCancelReservation.Click += new EventHandler(BtnCancelReservation_Click);

            // Add controls to form
            this.Controls.Add(this.tabControl);
        }

        private void LoadUsers()
        {
            // TODO: Implement loading users from database
            // This should populate dgvUsers with all users
        }

        private void LoadListings()
        {
            // TODO: Implement loading listings from database
            // This should populate dgvListings with all listings
        }

        private void LoadReservations()
        {
            // TODO: Implement loading reservations from database
            // This should populate dgvReservations with all reservations
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
            var response = _admin.FreezerAccount(userId);

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
    }
} 