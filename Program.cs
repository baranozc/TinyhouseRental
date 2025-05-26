using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject
{
    // Connection string - veritabanı hazır olduğunda güncellenecek
    public static class DatabaseConfig
    {
        public static string ConnectionString = "Server=localhost;Database=TinyHouseManagement;Trusted_Connection=True;";
    }

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

    public enum UserType
    {
        CLIENT,
        ADMIN
    }

    // Base User class
    public abstract class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserType UserType { get; set; }
        public bool UserState { get; set; }

        public void Login() {
         
        }
        public void Logout() { 

        }
    }

    // Client class
    public class Client : User // Yagiz.06 : Hepsine tek tek yazmaya üşendim, Baran bu classın altındaki SQL sorgularını ayarlar mısın
    {
        public UserResponse Login(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password AND UserState = 1 AND UserType = 0";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        this.UserID = Convert.ToInt32(reader["UserID"]);
                        this.Email = reader["Email"].ToString();
                        this.Password = reader["Password"].ToString();
                        this.Name = reader["Name"].ToString();
                        this.Surname = reader["Surname"].ToString();
                        this.UserType = UserType.CLIENT;
                        this.UserState = Convert.ToBoolean(reader["UserState"]);

                        #if DEBUG
                        Console.WriteLine($"[CLIENT] Login successful: {this.Name} {this.Surname}");
                        #endif

                        return new UserResponse 
                        { 
                            Success = true,
                            Message = $"Login successful: {this.Name} {this.Surname}",
                            User = this
                        };
                    }
                    else
                    {
                        #if DEBUG
                        Console.WriteLine("[CLIENT] Login failed");
                        #endif

                        return new UserResponse 
                        { 
                            Success = false,
                            Message = "Login failed",
                            User = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Login error: {ex.Message}");
                #endif

                return new UserResponse 
                { 
                    Success = false,
                    Message = "An error occurred during login",
                    User = null
                };
            }
        }

        public ReservationResponse MakeReservation(int listingId, DateTime checkInDate, DateTime checkOutDate)
        {
            try
            {
                if (checkOutDate <= checkInDate)
                {
                    return new ReservationResponse 
                    { 
                        Success = false,
                        Message = "Check-out date cannot be before check-in date",
                        Reservation = null
                    };
                }

                Reservation newReservation = new Reservation
                {
                    UserID = this.UserID,
                    ListingID = listingId,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate,
                    ReservationState = true
                };

                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Reservations (UserID, ListingID, CheckInDate, CheckOutDate, ReservationState) " +
                                   "VALUES (@UserID, @ListingID, @CheckInDate, @CheckOutDate, @ReservationState)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", newReservation.UserID);
                        command.Parameters.AddWithValue("@ListingID", newReservation.ListingID);
                        command.Parameters.AddWithValue("@CheckInDate", newReservation.CheckInDate);
                        command.Parameters.AddWithValue("@CheckOutDate", newReservation.CheckOutDate);
                        command.Parameters.AddWithValue("@ReservationState", newReservation.ReservationState);

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Reservation successful");
                            #endif

                            return new ReservationResponse 
                            { 
                                Success = true,
                                Message = "Reservation successful",
                                Reservation = newReservation
                            };
                        }
                        else
                        {
                            return new ReservationResponse 
                            { 
                                Success = false,
                                Message = "Failed to create reservation",
                                Reservation = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Reservation error: {ex.Message}");
                #endif

                return new ReservationResponse 
                { 
                    Success = false,
                    Message = "An error occurred while creating reservation",
                    Reservation = null
                };
            }
        }

        public void ListRentals()
        {
            List<Listing> userListings = new List<Listing>();

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                connection.Open();

                // Yagiz.04 : Baran kanki üstte ne yazdıysam aynısı dsflksfdjlds eğer uygun değilse ayarlar mısın lütfen ^^
                string query = "SELECT ListingID, UserID, ListingTitle, RentalPrice, ListingState " +
                               "FROM Listings WHERE UserID = @UserID AND ListingState = 1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", this.UserID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Listing listing = new Listing
                            {
                                ListingID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                ListingTitle = reader.GetString(2),
                                RentalPrice = reader.GetDouble(3),
                                ListingState = reader.GetBoolean(4)
                            };

                            userListings.Add(listing);
                        }
                    }
                }
            }

            
            Console.WriteLine("O=== Aktif Ilanlariniz ===O");
            if (userListings.Count == 0)
            {
                Console.WriteLine("Hic ilaniniz bulunmamaktadir.");
            }
            else
            {
                foreach (var listing in userListings)
                {
                    Console.WriteLine($"ID: {listing.ListingID} | Baslik: {listing.ListingTitle} | Fiyat: {listing.RentalPrice} ₺");
                }
            }
        }

        public void ListReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                connection.Open();

                string query = "SELECT ReservationID, UserID, ListingID, PaymentID, CheckInDate, CheckOutDate, ReservationState " +
                               "FROM Reservations WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", this.UserID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservation res = new Reservation
                            {
                                ReservationID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                ListingID = reader.GetInt32(2),
                                PaymentID = reader.GetInt32(3),
                                CheckInDate = reader.GetDateTime(4),
                                CheckOutDate = reader.GetDateTime(5),
                                ReservationState = reader.GetBoolean(6)
                            };

                            reservations.Add(res);
                        }
                    }
                }
            }

            // Rezervasyon yazdıran zımbırtı
            Console.WriteLine("O=== Rezervasyonlariniz ===O");
            if (reservations.Count == 0)
            {
                Console.WriteLine("Herhangi bir rezervasyonunuz bulunmuyor.");
            }
            else
            {
                foreach (var r in reservations)
                {
                    Console.WriteLine($"Rezervasyon ID: {r.ReservationID} | Listing ID: {r.ListingID} | Giris : {r.CheckInDate.ToShortDateString()} | Cikis : {r.CheckOutDate.ToShortDateString()} | Durum : {(r.ReservationState ? "Aktif" : "Iptal")}");
                }
            }
        }

        public CommentResponse MakeComment(int listingId, string commentContent)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Comments (ListingID, UserID, CommentContent) " +
                                   "VALUES (@ListingID, @UserID, @CommentContent)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ListingID", listingId);
                        command.Parameters.AddWithValue("@UserID", this.UserID);
                        command.Parameters.AddWithValue("@CommentContent", commentContent);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Comment added successfully");
                            #endif

                            return new CommentResponse 
                            { 
                                Success = true,
                                Message = "Comment added successfully",
                                Comment = new Comment 
                                { 
                                    ListingID = listingId,
                                    UserID = this.UserID,
                                    CommentContent = commentContent
                                }
                            };
                        }
                        else
                        {
                            return new CommentResponse 
                            { 
                                Success = false,
                                Message = "Failed to add comment",
                                Comment = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Add comment error: {ex.Message}");
                #endif

                return new CommentResponse 
                { 
                    Success = false,
                    Message = "An error occurred while adding comment",
                    Comment = null
                };
            }
        }

        public ListingResponse CreateListing(string title, double price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Listings (UserID, ListingTitle, RentalPrice, ListingState) " +
                                   "VALUES (@UserID, @Title, @Price, @State)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", this.UserID);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@State", true);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Listing created successfully");
                            #endif

                            return new ListingResponse 
                            { 
                                Success = true,
                                Message = "Listing created successfully",
                                Listing = new Listing 
                                { 
                                    UserID = this.UserID,
                                    ListingTitle = title,
                                    RentalPrice = price,
                                    ListingState = true
                                }
                            };
                        }
                        else
                        {
                            return new ListingResponse 
                            { 
                                Success = false,
                                Message = "Failed to create listing",
                                Listing = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Create listing error: {ex.Message}");
                #endif

                return new ListingResponse 
                { 
                    Success = false,
                    Message = "An error occurred while creating listing",
                    Listing = null
                };
            }
        }

        public ListingResponse DeactivateListing(int listingId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Listings SET ListingState = @State WHERE ListingID = @ListingID AND UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", false);
                        command.Parameters.AddWithValue("@ListingID", listingId);
                        command.Parameters.AddWithValue("@UserID", this.UserID);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Listing deactivated successfully");
                            #endif

                            return new ListingResponse 
                            { 
                                Success = true,
                                Message = "Listing deactivated successfully",
                                Listing = null
                            };
                        }
                        else
                        {
                            return new ListingResponse 
                            { 
                                Success = false,
                                Message = "Failed to deactivate listing",
                                Listing = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Deactivate listing error: {ex.Message}");
                #endif

                return new ListingResponse 
                { 
                    Success = false,
                    Message = "An error occurred while deactivating listing",
                    Listing = null
                };
            }
        }

        public bool UpdateListing(int listingId, string newTitle, double newPrice)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Listings SET ListingTitle = @Title, RentalPrice = @Price " +
                                   "WHERE ListingID = @ListingID AND UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", newTitle);
                        command.Parameters.AddWithValue("@Price", newPrice);
                        command.Parameters.AddWithValue("@ListingID", listingId);
                        command.Parameters.AddWithValue("@UserID", this.UserID);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("İlan güncellenirken hata oluştu: " + ex.Message);
                    return false;
                }
            }
        }

        public void ShowComments() { 

        }
    }

    public class Admin : User
    {
        public UserResponse Login(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password AND UserState = 1 AND UserType = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        this.UserID = Convert.ToInt32(reader["UserID"]);
                        this.Email = reader["Email"].ToString();
                        this.Password = reader["Password"].ToString();
                        this.Name = reader["Name"].ToString();
                        this.Surname = reader["Surname"].ToString();
                        this.UserType = UserType.ADMIN;
                        this.UserState = Convert.ToBoolean(reader["UserState"]);

                        #if DEBUG
                        Console.WriteLine($"[ADMIN] Login successful: {this.Name} {this.Surname}");
                        #endif

                        return new UserResponse 
                        { 
                            Success = true,
                            Message = $"Login successful: {this.Name} {this.Surname}",
                            User = this
                        };
                    }
                    else
                    {
                        #if DEBUG
                        Console.WriteLine("[ADMIN] Login failed");
                        #endif

                        return new UserResponse 
                        { 
                            Success = false,
                            Message = "Login failed",
                            User = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Login error: {ex.Message}");
                #endif

                return new UserResponse 
                { 
                    Success = false,
                    Message = "An error occurred during login",
                    User = null
                };
            }
        }

        public UserResponse FreezerAccount(int targetUserId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Users SET UserState = @State WHERE UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", false);
                        command.Parameters.AddWithValue("@UserID", targetUserId);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Account frozen successfully");
                            #endif

                            return new UserResponse 
                            { 
                                Success = true,
                                Message = "Account frozen successfully",
                                User = null
                            };
                        }
                        else
                        {
                            return new UserResponse 
                            { 
                                Success = false,
                                Message = "Failed to freeze account",
                                User = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Freeze account error: {ex.Message}");
                #endif

                return new UserResponse 
                { 
                    Success = false,
                    Message = "An error occurred while freezing account",
                    User = null
                };
            }
        }

        public ListingResponse DeactivateListing(int listingId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Listings SET ListingState = @State WHERE ListingID = @ListingID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", false);
                        command.Parameters.AddWithValue("@ListingID", listingId);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Listing deactivated successfully");
                            #endif

                            return new ListingResponse 
                            { 
                                Success = true,
                                Message = "Listing deactivated successfully",
                                Listing = null
                            };
                        }
                        else
                        {
                            return new ListingResponse 
                            { 
                                Success = false,
                                Message = "Failed to deactivate listing",
                                Listing = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Deactivate listing error: {ex.Message}");
                #endif

                return new ListingResponse 
                { 
                    Success = false,
                    Message = "An error occurred while deactivating listing",
                    Listing = null
                };
            }
        }

        public ListingResponse ActivateListing(int listingId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Listings SET ListingState = @State WHERE ListingID = @ListingID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", true);
                        command.Parameters.AddWithValue("@ListingID", listingId);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Listing activated successfully");
                            #endif

                            return new ListingResponse 
                            { 
                                Success = true,
                                Message = "Listing activated successfully",
                                Listing = null
                            };
                        }
                        else
                        {
                            return new ListingResponse 
                            { 
                                Success = false,
                                Message = "Failed to activate listing",
                                Listing = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Activate listing error: {ex.Message}");
                #endif

                return new ListingResponse 
                { 
                    Success = false,
                    Message = "An error occurred while activating listing",
                    Listing = null
                };
            }
        }

        public ReservationResponse CancelReservation(int reservationId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Reservations SET ReservationState = @State WHERE ReservationID = @ReservationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", false);
                        command.Parameters.AddWithValue("@ReservationID", reservationId);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Reservation cancelled successfully");
                            #endif

                            return new ReservationResponse 
                            { 
                                Success = true,
                                Message = "Reservation cancelled successfully",
                                Reservation = null
                            };
                        }
                        else
                        {
                            return new ReservationResponse 
                            { 
                                Success = false,
                                Message = "Failed to cancel reservation",
                                Reservation = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Cancel reservation error: {ex.Message}");
                #endif

                return new ReservationResponse 
                { 
                    Success = false,
                    Message = "An error occurred while cancelling reservation",
                    Reservation = null
                };
            }
        }

        public ReservationResponse ApproveReservation(int reservationId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Reservations SET ReservationState = @State WHERE ReservationID = @ReservationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", true);
                        command.Parameters.AddWithValue("@ReservationID", reservationId);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Reservation approved successfully");
                            #endif

                            return new ReservationResponse 
                            { 
                                Success = true,
                                Message = "Reservation approved successfully",
                                Reservation = null
                            };
                        }
                        else
                        {
                            return new ReservationResponse 
                            { 
                                Success = false,
                                Message = "Failed to approve reservation",
                                Reservation = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Approve reservation error: {ex.Message}");
                #endif

                return new ReservationResponse 
                { 
                    Success = false,
                    Message = "An error occurred while approving reservation",
                    Reservation = null
                };
            }
        }

        public CommentResponse DeleteComment(int commentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Comments WHERE CommentID = @CommentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CommentID", commentId);

                        int result = command.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            #if DEBUG
                            Console.WriteLine("Comment deleted successfully");
                            #endif

                            return new CommentResponse 
                            { 
                                Success = true,
                                Message = "Comment deleted successfully",
                                Comment = null
                            };
                        }
                        else
                        {
                            return new CommentResponse 
                            { 
                                Success = false,
                                Message = "Failed to delete comment",
                                Comment = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                Console.WriteLine($"Delete comment error: {ex.Message}");
                #endif

                return new CommentResponse 
                { 
                    Success = false,
                    Message = "An error occurred while deleting comment",
                    Comment = null
                };
            }
        }
    }

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
}

