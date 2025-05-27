using System;
using System.Windows.Forms;

namespace MyProject.Forms
{
    public partial class ClientDashboard : Form
    {
        private Client _client;
        private TabControl tabControl;
        private TabPage tabListings;
        private TabPage tabReservations;
        private TabPage tabProfile;
        private DataGridView dgvListings;
        private DataGridView dgvReservations;
        private Button btnMakeReservation;
        private Button btnViewDetails;
        private Button btnCancelReservation;

        public ClientDashboard(Client client)
        {
            _client = client;
            InitializeComponent();
            LoadListings();
            LoadReservations();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabListings = new TabPage();
            this.tabReservations = new TabPage();
            this.tabProfile = new TabPage();
            this.dgvListings = new DataGridView();
            this.dgvReservations = new DataGridView();
            this.btnMakeReservation = new Button();
            this.btnViewDetails = new Button();
            this.btnCancelReservation = new Button();

            // Form
            this.Text = "Tiny House Rental - Client Dashboard";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tab Control
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Controls.AddRange(new Control[] {
                this.tabListings,
                this.tabReservations,
                this.tabProfile
            });

            // Listings Tab
            this.tabListings.Text = "Available Listings";
            this.tabListings.Controls.Add(this.dgvListings);
            this.tabListings.Controls.Add(this.btnMakeReservation);
            this.tabListings.Controls.Add(this.btnViewDetails);

            // Listings DataGridView
            this.dgvListings.Dock = DockStyle.Top;
            this.dgvListings.Height = 400;
            this.dgvListings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvListings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvListings.MultiSelect = false;

            // Make Reservation Button
            this.btnMakeReservation.Text = "Make Reservation";
            this.btnMakeReservation.Location = new System.Drawing.Point(10, 420);
            this.btnMakeReservation.Size = new System.Drawing.Size(120, 30);
            this.btnMakeReservation.Click += new EventHandler(BtnMakeReservation_Click);

            // View Details Button
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.Location = new System.Drawing.Point(140, 420);
            this.btnViewDetails.Size = new System.Drawing.Size(120, 30);
            this.btnViewDetails.Click += new EventHandler(BtnViewDetails_Click);

            // Reservations Tab
            this.tabReservations.Text = "My Reservations";
            this.tabReservations.Controls.Add(this.dgvReservations);
            this.tabReservations.Controls.Add(this.btnCancelReservation);

            // Reservations DataGridView
            this.dgvReservations.Dock = DockStyle.Top;
            this.dgvReservations.Height = 400;
            this.dgvReservations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservations.MultiSelect = false;

            // Cancel Reservation Button
            this.btnCancelReservation.Text = "Cancel Reservation";
            this.btnCancelReservation.Location = new System.Drawing.Point(10, 420);
            this.btnCancelReservation.Size = new System.Drawing.Size(120, 30);
            this.btnCancelReservation.Click += new EventHandler(BtnCancelReservation_Click);

            // Profile Tab
            this.tabProfile.Text = "My Profile";
            // TODO: Add profile information and edit functionality

            // Add controls to form
            this.Controls.Add(this.tabControl);
        }

        private void LoadListings()
        {
            // TODO: Implement loading listings from database
            // This should populate dgvListings with available listings
        }

        private void LoadReservations()
        {
            // TODO: Implement loading user's reservations from database
            // This should populate dgvReservations with user's reservations
        }

        private void BtnMakeReservation_Click(object sender, EventArgs e)
        {
            if (dgvListings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a listing first.", "Make Reservation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Implement reservation creation
            // Show a dialog for selecting dates and confirming reservation
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

            if (MessageBox.Show("Are you sure you want to cancel this reservation?", "Cancel Reservation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // TODO: Implement reservation cancellation
                // Call the appropriate method to cancel the reservation
            }
        }
    }
} 