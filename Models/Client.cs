using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MyProject.Config;

namespace MyProject.Models
{
    public class Client : User
    {
        public override UserResponse Login(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password AND UserState = True AND UserTypeId = 1";

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
                        this.UserTypeId = UserTypeId.CLIENT;
                        this.UserState = Convert.ToBoolean(reader["UserState"]);

                        return new UserResponse 
                        { 
                            Success = true,
                            Message = $"Login successful: {this.Name} {this.Surname}",
                            User = this
                        };
                    }
                    else
                    {
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
                return new UserResponse 
                { 
                    Success = false,
                    Message = "An error occurred during login",
                    User = null
                };
            }
        }

        public override UserResponse Logout()
        {
            try
            {
                // Kullanıcı bilgilerini sıfırla
                string userName = $"{this.Name} {this.Surname}"; // Mesaj için kullanıcı adını sakla
                this.UserID = 0;
                this.Email = null;
                this.Password = null;
                this.Name = null;
                this.Surname = null;
                this.UserTypeId = UserTypeId.CLIENT;
                this.UserState = false;

                return new UserResponse
                {
                    Success = true,
                    Message = $"Logout successful: {userName}",
                    User = null
                };
            }
            catch (Exception ex)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = $"An error occurred during logout: {ex.Message}",
                    User = this
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

                // Check if the listing is available for these dates
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    
                    // First check if there are any overlapping reservations
                    string checkQuery = @"SELECT COUNT(*) FROM Reservations 
                                       WHERE ListingID = @ListingID 
                                       AND ReservationState = 1
                                       AND ((CheckInDate <= @CheckOutDate AND CheckOutDate >= @CheckInDate)
                                       OR (CheckInDate <= @CheckInDate AND CheckOutDate >= @CheckInDate)
                                       OR (CheckInDate <= @CheckOutDate AND CheckOutDate >= @CheckOutDate))";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@ListingID", listingId);
                        checkCommand.Parameters.AddWithValue("@CheckInDate", checkInDate);
                        checkCommand.Parameters.AddWithValue("@CheckOutDate", checkOutDate);

                        int overlappingReservations = (int)checkCommand.ExecuteScalar();
                        
                        if (overlappingReservations > 0)
                        {
                            return new ReservationResponse 
                            { 
                                Success = false,
                                Message = "This listing is already reserved for the selected dates",
                                Reservation = null
                            };
                        }
                    }

                    // If no overlapping reservations, create the new reservation
                    string insertQuery = @"INSERT INTO Reservations 
                                         (UserID, ListingID, CheckInDate, CheckOutDate, ReservationState, 
                                          IsPaid) 
                                         VALUES 
                                         (@UserID, @ListingID, @CheckInDate, @CheckOutDate, @ReservationState,
                                          @IsPaid)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", this.UserID);
                        command.Parameters.AddWithValue("@ListingID", listingId);
                        command.Parameters.AddWithValue("@CheckInDate", checkInDate);
                        command.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
                        command.Parameters.AddWithValue("@ReservationState", true);
                        command.Parameters.AddWithValue("@IsPaid", false); // Başlangıçta ödeme yapılmamış olarak işaretlenir

                        try
                        {
                            int result = command.ExecuteNonQuery();

                            if (result > 0)
                            {
                                return new ReservationResponse 
                                { 
                                    Success = true,
                                    Message = "Reservation successful",
                                    Reservation = new Reservation
                                    {
                                        UserID = this.UserID,
                                        ListingID = listingId,
                                        CheckInDate = checkInDate,
                                        CheckOutDate = checkOutDate,
                                        ReservationState = true,
                                        IsPaid = false
                                    }
                                };
                            }
                            else
                            {
                                return new ReservationResponse 
                                { 
                                    Success = false,
                                    Message = "Failed to create reservation - no rows affected",
                                    Reservation = null
                                };
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            return new ReservationResponse 
                            { 
                                Success = false,
                                Message = $"Database error: {sqlEx.Message}\nSQL State: {sqlEx.State}\nError Code: {sqlEx.Number}",
                                Reservation = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ReservationResponse 
                { 
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}\nStack trace: {ex.StackTrace}",
                    Reservation = null
                };
            }
        }

        // Diğer metodlar buraya eklenecek...
    }
} 