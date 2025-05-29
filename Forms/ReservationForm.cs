using System;
using System.Windows.Forms;
using MyProject.Models;
using System.Drawing;

namespace MyProject.Forms
{
    public partial class ReservationForm : Form
    {
        private readonly Client _client;
        private readonly DataGridViewRow _selectedListing;
        private DateTimePicker dtpCheckIn;
        private DateTimePicker dtpCheckOut;
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblCheckIn;
        private Label lblCheckOut;
        private Label lblListingInfo;
        private Label lblTotalPrice;

        public ReservationForm(Client client, DataGridViewRow selectedListing)
        {
            _client = client;
            _selectedListing = selectedListing;
            InitializeComponent();
            DisplayListingInfo();
        }

        private void InitializeComponent()
        {
            this.dtpCheckIn = new DateTimePicker();
            this.dtpCheckOut = new DateTimePicker();
            this.btnConfirm = new Button();
            this.btnCancel = new Button();
            this.lblCheckIn = new Label();
            this.lblCheckOut = new Label();
            this.lblListingInfo = new Label();
            this.lblTotalPrice = new Label();

            // Form
            this.Text = "Make Reservation";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Listing Info Label
            this.lblListingInfo.Location = new Point(20, 20);
            this.lblListingInfo.Size = new Size(360, 60);
            this.lblListingInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Check-in Label and DateTimePicker
            this.lblCheckIn.Text = "Check-in Date:";
            this.lblCheckIn.Location = new Point(20, 100);
            this.lblCheckIn.AutoSize = true;

            this.dtpCheckIn.Location = new Point(20, 120);
            this.dtpCheckIn.Size = new Size(200, 25);
            this.dtpCheckIn.MinDate = DateTime.Today;
            this.dtpCheckIn.ValueChanged += new EventHandler(Dates_ValueChanged);

            // Check-out Label and DateTimePicker
            this.lblCheckOut.Text = "Check-out Date:";
            this.lblCheckOut.Location = new Point(20, 160);
            this.lblCheckOut.AutoSize = true;

            this.dtpCheckOut.Location = new Point(20, 180);
            this.dtpCheckOut.Size = new Size(200, 25);
            this.dtpCheckOut.MinDate = DateTime.Today.AddDays(1);
            this.dtpCheckOut.ValueChanged += new EventHandler(Dates_ValueChanged);

            // Total Price Label
            this.lblTotalPrice.Location = new Point(20, 240);
            this.lblTotalPrice.Size = new Size(340, 25);
            this.lblTotalPrice.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTotalPrice.TextAlign = ContentAlignment.MiddleRight;

            // Confirm Button
            this.btnConfirm.Text = "Confirm Reservation";
            this.btnConfirm.Location = new Point(20, 300);
            this.btnConfirm.Size = new Size(160, 35);
            this.btnConfirm.BackColor = Color.FromArgb(40, 167, 69);
            this.btnConfirm.ForeColor = Color.White;
            this.btnConfirm.FlatStyle = FlatStyle.Flat;
            this.btnConfirm.Click += new EventHandler(BtnConfirm_Click);

            // Cancel Button
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new Point(200, 300);
            this.btnCancel.Size = new Size(160, 35);
            this.btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                this.lblListingInfo,
                this.lblCheckIn,
                this.dtpCheckIn,
                this.lblCheckOut,
                this.dtpCheckOut,
                this.lblTotalPrice,
                this.btnConfirm,
                this.btnCancel
            });
        }

        private void DisplayListingInfo()
        {
            string title = _selectedListing.Cells["listingTitle"].Value.ToString();
            string owner = _selectedListing.Cells["ownerName"].Value.ToString();
            decimal price = Convert.ToDecimal(_selectedListing.Cells["rentalPrice"].Value);

            lblListingInfo.Text = $"Title: {title}\nOwner: {owner}\nPrice per night: ₺{price:N2}";
            UpdateTotalPrice();
        }

        private void Dates_ValueChanged(object sender, EventArgs e)
        {
            if (dtpCheckOut.Value <= dtpCheckIn.Value)
            {
                dtpCheckOut.Value = dtpCheckIn.Value.AddDays(1);
            }
            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            decimal pricePerNight = Convert.ToDecimal(_selectedListing.Cells["rentalPrice"].Value);
            int numberOfNights = (dtpCheckOut.Value - dtpCheckIn.Value).Days;
            decimal totalPrice = pricePerNight * numberOfNights;

            lblTotalPrice.Text = $"Total Price: ₺{totalPrice:N2}";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int listingId = Convert.ToInt32(_selectedListing.Cells["listingID"].Value);
                var response = _client.MakeReservation(listingId, dtpCheckIn.Value, dtpCheckOut.Value);

                if (response.Success)
                {
                    MessageBox.Show("Reservation created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Failed to create reservation: {response.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 