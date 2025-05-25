using System;
using System.Data.SqlClient;
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
    private string connectionString = "Server=localhost;Database=TinyHouseDB;Trusted_Connection=True;";
    // Yagiz.01 : Burayı bizim SQL sunucuya göre düzenler misiniz 

    public User Login(string email, string password)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password AND UserState = 1 AND UserType = 0";
            //Yagiz.02 : Aynı şekilde burayı da, Database hazır değildi bunu yazarken 


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

                Console.WriteLine($"[CLIENT] Giriş başarılı: {this.Name} {this.Surname}");
                return this;
            }
            else
            {
                Console.WriteLine("[CLIENT] Hatali giris.");
                return null;
            }
        }
    }

    public bool MakeReservation(int listingId, DateTime checkInDate, DateTime checkOutDate)
    {
        // Rezervasyon için kontrol yapıyo bura, yani bitiş tarihi başlangıç tarihinden önce olursa hata vecek
        if (checkOutDate <= checkInDate)
        {
            Console.WriteLine("Cikis tarihi giristen once olamaz...");
            Console.Beep();
            return false;
        }

        // Rezervasyon için nesne oluşturuyos
        Reservation newReservation = new Reservation
        {
            UserID = this.UserID,  // Client sınıfındaki UserID buradaki
            ListingID = listingId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            ReservationState = true  // Burada reservasyon durumunu ilk aktif ypaıyoz
        };

        // Veritabanı bağlantısı için gerekli SQL nesnesi
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // SQL sorgusunu oluşturan yer, Baran burayı bizim veritabanına göre düzenler misin
            string query = "INSERT INTO Reservations (UserID, ListingID, CheckInDate, CheckOutDate, ReservationState) " +
                           "VALUES (@UserID, @ListingID, @CheckInDate, @CheckOutDate, @ReservationState)";

            
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Sorguya eklencek parametreler
                command.Parameters.AddWithValue("@UserID", newReservation.UserID);
                command.Parameters.AddWithValue("@ListingID", newReservation.ListingID);
                command.Parameters.AddWithValue("@CheckInDate", newReservation.CheckInDate);
                command.Parameters.AddWithValue("@CheckOutDate", newReservation.CheckOutDate);
                command.Parameters.AddWithValue("@ReservationState", newReservation.ReservationState);

                // Sorgu çalıştırılıyor ve etkilenen satır sayısı kontrol ediliyor
                int result = command.ExecuteNonQuery();

                // Eğer işlem başarılı ise
                if (result > 0)
                {
                    Console.WriteLine("Rezervasyon başarılı.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Rezervasyon oluşturulurken bir hata oluştu.");
                    return false;
                }
            }
        }
    }
    public void ListRentals()
{
    List<Listing> userListings = new List<Listing>();

    using (SqlConnection connection = new SqlConnection(connectionString))
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

    using (SqlConnection connection = new SqlConnection(connectionString))
    {1
        connection.Open();

        // Yagiz.05 : Bam du bom du bam du bom du du bam du bom du bam du bom du 
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

    public bool MakeComment(int listingId, string commentContent)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
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
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Yorum yapilirken bir hata olustu! : " + ex.Message);
            return false;
        }
    }
}

    public bool CreateListing(string title, double price)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
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
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("İlan oluşturulurken hata oluştu: " + ex.Message);
            return false;
        }
    }
}

    public bool DeactivateListing(int listingId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "UPDATE Listings SET ListingState = @State " +
                           "WHERE ListingID = @ListingID AND UserID = @UserID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@State", false);
                command.Parameters.AddWithValue("@ListingID", listingId);
                command.Parameters.AddWithValue("@UserID", this.UserID);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ilan pasif hale getirilirken hata olustu: " + ex.Message);
            return false;
        }
    }
}

    public bool UpdateListing(int listingId, string newTitle, double newPrice)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
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
    private string connectionString = "Server=localhost;Database=TinyHouseDB;Trusted_Connection=True;";

    public User Login(string email, string password)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
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

                Console.WriteLine($"[ADMIN] Giriş başarılı: {this.Name} {this.Surname}");
                return this;
            }
            else
            {
                Console.WriteLine("[ADMIN] Hatalı giriş.");
                return null;
            }
        }
    }

    public bool FreezerAccount(int targetUserId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "UPDATE Users SET UserState = @State WHERE UserID = @UserID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@State", false); // Hesabı dondur
                command.Parameters.AddWithValue("@UserID", targetUserId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Kullanıcı hesabı dondurulurken hata oluştu: " + ex.Message);
            return false;
        }
    }
}

    public bool DeactivateListing(int listingId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "UPDATE Listings SET ListingState = @State WHERE ListingID = @ListingID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@State", false); // Yayından kaldır
                command.Parameters.AddWithValue("@ListingID", listingId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("İlan pasif hale getirilirken hata oluştu: " + ex.Message);
            return false;
        }
    }
}

    public bool ActivateListing(int listingId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "UPDATE Listings SET ListingState = @State WHERE ListingID = @ListingID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@State", true); // İlanı aktif hâle getir
                command.Parameters.AddWithValue("@ListingID", listingId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("İlan aktive edilirken hata oluştu: " + ex.Message);
            return false;
        }
    }
}

    public bool CancelReservation(int reservationId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "UPDATE Reservations SET ReservationState = @State WHERE ReservationID = @ReservationID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@State", false); // Rezervasyonu iptal et
                command.Parameters.AddWithValue("@ReservationID", reservationId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Rezervasyon iptal edilirken hata oluştu: " + ex.Message);
            return false;
        }
    }
}

    public bool ApproveReservation(int reservationId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "UPDATE Reservations SET ReservationState = @State WHERE ReservationID = @ReservationID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@State", true); // Rezervasyonu onayla
                command.Parameters.AddWithValue("@ReservationID", reservationId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Rezervasyon onaylanırken hata oluştu: " + ex.Message);
            return false;
        }
    }
}

    public List<Comment> ShowComments(int listingId)
{
    List<Comment> comments = new List<Comment>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "SELECT CommentID, UserID, ListingID, CommentContent FROM Comments WHERE ListingID = @ListingID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ListingID", listingId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            CommentID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            ListingID = reader.GetInt32(2),
                            CommentContent = reader.GetString(3)
                        };
                        comments.Add(comment);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Yorumlar listelenirken hata oluştu: " + ex.Message);
        }
    }

    return comments;
}

    public bool DeleteComment(int commentId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            string query = "DELETE FROM Comments WHERE CommentID = @CommentID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CommentID", commentId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Yorum silinirken hata oluştu: " + ex.Message);
            return false;
        }
    }
}

}

// Yagiz 1.1 : Buraya bişi eklemek aklıma gelmedi öylesine metod yazdım bişiler ekler misin @Burak
public class Listing
{
    public int ListingID { get; set; }
    public int UserID { get; set; }
    public string ListingTitle { get; set; }
    public double RentalPrice { get; set; }
    public List<Comment> ListingComments { get; set; }
    public bool ListingState { get; set; }

    public void Method(string type) { }
}

// Yagiz 1.2 : Buraya da eklemek aklıma gelmedi bişi bulur musun @Bayram
public class Comment
{
    public int ListingID { get; set; }
    public int UserID { get; set; }
    public int CommentID { get; set; }
    public string CommentContent { get; set; }

    public void Method(string type) { }
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
}

// Payment class
public class Payment
{
    public int PaymentID { get; set; }
    public int UserID { get; set; }
    public int ListingID { get; set; }
    public int PaymentAmount { get; set; }
}
