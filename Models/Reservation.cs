using System;

namespace MyProject.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int UserID { get; set; }
        public int ListingID { get; set; }
        public int PaymentID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool ReservationState { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancellationReason { get; set; }
        public double TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public string SpecialRequests { get; set; }
        public int GuestCount { get; set; }
        public bool IsReviewed { get; set; }

        public Reservation()
        {
            CreatedDate = DateTime.Now;
            ReservationState = true;
            IsPaid = false;
            IsReviewed = false;
        }

        public void Cancel(string reason)
        {
            ReservationState = false;
            CancelledDate = DateTime.Now;
            CancellationReason = reason;
        }

        public void ConfirmPayment()
        {
            IsPaid = true;
        }

        public void AddReview()
        {
            IsReviewed = true;
        }

        public int GetDuration()
        {
            return (CheckOutDate - CheckInDate).Days;
        }

        public bool IsActive()
        {
            return ReservationState && !IsCancelled() && CheckOutDate > DateTime.Now;
        }

        public bool IsCancelled()
        {
            return CancelledDate.HasValue;
        }

        public bool IsUpcoming()
        {
            return ReservationState && !IsCancelled() && CheckInDate > DateTime.Now;
        }

        public bool IsPast()
        {
            return CheckOutDate < DateTime.Now;
        }

        public void UpdateDates(DateTime newCheckIn, DateTime newCheckOut)
        {
            if (newCheckOut > newCheckIn)
            {
                CheckInDate = newCheckIn;
                CheckOutDate = newCheckOut;
            }
        }

        public void UpdateGuestCount(int newCount)
        {
            if (newCount > 0)
            {
                GuestCount = newCount;
            }
        }

        public void AddSpecialRequest(string request)
        {
            if (string.IsNullOrEmpty(SpecialRequests))
            {
                SpecialRequests = request;
            }
            else
            {
                SpecialRequests += $"\n{request}";
            }
        }

        public override string ToString()
        {
            return $"Reservation #{ReservationID} - {CheckInDate:d} to {CheckOutDate:d} - {(IsPaid ? "Paid" : "Unpaid")}";
        }
    }
} 