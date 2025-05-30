using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MyProject.Forms;
using MyProject.Config;
using MyProject.Models;

namespace MyProject
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UserResponse : BaseResponse
    {
        public User User { get; set; }
    }

    public class ListingResponse : BaseResponse
    {
        public Listing Listing { get; set; }
    }

    public class ReservationResponse : BaseResponse
    {
        public Reservation Reservation { get; set; }
    }

    public class CommentResponse : BaseResponse
    {
        public Comment Comment { get; set; }
    }

    // Base User class
    // KULLANICI SINIFLARI BURADAN SİLİNDİ. TÜM KULLANICI SINIFLARI Models KLASÖRÜNDE OLMALI.

    // Client class
    // KULLANICI SINIFLARI BURADAN SİLİNDİ. TÜM KULLANICI SINIFLARI Models KLASÖRÜNDE OLMALI.

    // Yagiz 1.1 : Buraya bişi eklemek aklıma gelmedi öylesine metod yazdım bişiler ekler misin @Burak
    public class Listing
    {
        public int ListingID { get; set; }
        public int UserID { get; set; }
        public string ListingTitle { get; set; }
        public string Description { get; set; }
        public double RentalPrice { get; set; }
        public string Location { get; set; }
        public int BedroomCount { get; set; }
        public int BathroomCount { get; set; }
        public double SquareMeters { get; set; }
        public List<string> Amenities { get; set; }
        public List<Comment> ListingComments { get; set; }
        public bool ListingState { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public List<string> ImageUrls { get; set; }
        public double Rating { get; set; }
        public int ViewCount { get; set; }

        public Listing()
        {
            Amenities = new List<string>();
            ListingComments = new List<Comment>();
            ImageUrls = new List<string>();
            CreatedDate = DateTime.Now;
            LastUpdatedDate = DateTime.Now;
            ViewCount = 0;
            Rating = 0;
        }

        public void AddAmenity(string amenity)
        {
            if (!Amenities.Contains(amenity))
            {
                Amenities.Add(amenity);
                LastUpdatedDate = DateTime.Now;
            }
        }

        public void RemoveAmenity(string amenity)
        {
            if (Amenities.Contains(amenity))
            {
                Amenities.Remove(amenity);
                LastUpdatedDate = DateTime.Now;
            }
        }

        public void AddImage(string imageUrl)
        {
            if (!ImageUrls.Contains(imageUrl))
            {
                ImageUrls.Add(imageUrl);
                LastUpdatedDate = DateTime.Now;
            }
        }

        public void RemoveImage(string imageUrl)
        {
            if (ImageUrls.Contains(imageUrl))
            {
                ImageUrls.Remove(imageUrl);
                LastUpdatedDate = DateTime.Now;
            }
        }

        public void IncrementViewCount()
        {
            ViewCount++;
        }

        public void UpdateRating(double newRating)
        {
            if (newRating >= 0 && newRating <= 5)
            {
                Rating = newRating;
                LastUpdatedDate = DateTime.Now;
            }
        }

        public bool IsAvailable(DateTime checkIn, DateTime checkOut)
        {
            if (checkOut <= checkIn)
                return false;

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) FROM Reservations 
                               WHERE ListingID = @ListingID 
                               AND ReservationState = 1
                               AND ((CheckInDate <= @CheckOut AND CheckOutDate >= @CheckIn)
                               OR (CheckInDate <= @CheckIn AND CheckOutDate >= @CheckIn)
                               OR (CheckInDate <= @CheckOut AND CheckOutDate >= @CheckOut))";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ListingID", ListingID);
                    command.Parameters.AddWithValue("@CheckIn", checkIn);
                    command.Parameters.AddWithValue("@CheckOut", checkOut);

                    int reservationCount = (int)command.ExecuteScalar();
                    return reservationCount == 0;
                }
            }
        }

        public double CalculateTotalPrice(DateTime checkIn, DateTime checkOut)
        {
            TimeSpan duration = checkOut - checkIn;
            int days = duration.Days;
            return RentalPrice * days;
        }

        public string GetStatus()
        {
            return ListingState ? "Active" : "Inactive";
        }

        public void UpdateDetails(string title, string description, double price, string location)
        {
            ListingTitle = title;
            Description = description;
            RentalPrice = price;
            Location = location;
            LastUpdatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{ListingTitle} - {Location} - {RentalPrice:C} per day";
        }
    }

    // Yagiz 1.2 : Buraya da eklemek aklıma gelmedi bişi bulur musun @Bayram
    public class Comment
    {
        public int CommentID { get; set; }
        public int ListingID { get; set; }
        public int UserID { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsEdited { get; set; }
        public bool IsReported { get; set; }
        public List<Comment> Replies { get; set; }
        public int? ParentCommentID { get; set; }

        public Comment()
        {
            CreatedDate = DateTime.Now;
            Replies = new List<Comment>();
            LikeCount = 0;
            DislikeCount = 0;
            IsEdited = false;
            IsReported = false;
        }

        public void EditComment(string newContent)
        {
            CommentContent = newContent;
            LastEditedDate = DateTime.Now;
            IsEdited = true;
        }

        public void AddReply(Comment reply)
        {
            reply.ParentCommentID = this.CommentID;
            Replies.Add(reply);
        }

        public void RemoveReply(int replyId)
        {
            Replies.RemoveAll(r => r.CommentID == replyId);
        }

        public void Like()
        {
            LikeCount++;
        }

        public void Dislike()
        {
            DislikeCount++;
        }

        public void Report()
        {
            IsReported = true;
        }

        public void Unreport()
        {
            IsReported = false;
        }

        public double GetRating()
        {
            if (LikeCount + DislikeCount == 0)
                return 0;
            return (double)LikeCount / (LikeCount + DislikeCount) * 5;
        }

        public override string ToString()
        {
            return $"{CommentContent} - {CreatedDate:g}";
        }
    }

    // Reservation class

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

    // Payment class
    public class Payment
    {
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public int ListingID { get; set; }
        public int PaymentAmount { get; set; }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}

