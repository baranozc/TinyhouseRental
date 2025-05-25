using System;

namespace MyProject.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public int ListingID { get; set; }
        public int PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        public bool IsRefunded { get; set; }
        public DateTime? RefundDate { get; set; }
        public string RefundReason { get; set; }

        public Payment()
        {
            PaymentDate = DateTime.Now;
            IsRefunded = false;
        }

        public void ProcessRefund(string reason)
        {
            IsRefunded = true;
            RefundDate = DateTime.Now;
            RefundReason = reason;
        }

        public override string ToString()
        {
            return $"Payment #{PaymentID} - {PaymentAmount:C} - {PaymentDate:g}";
        }
    }
} 