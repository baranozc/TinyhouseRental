using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MyProject.Config;

namespace MyProject.Models
{
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
} 