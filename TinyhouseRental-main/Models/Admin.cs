using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MyProject.Config;

namespace MyProject.Models
{
    public class Admin : User
    {
        public override UserResponse Login(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password AND UserState = True AND UserTypeId = 0";

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
                        this.UserTypeId = UserTypeId.ADMIN;
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
                            Message = "Email veya şifre hatalı",
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
                    Message = $"Beklenmeyen bir hata oluştu: {ex.Message}",
                    User = null
                };
            }
        }

        public override UserResponse Logout()
        {
            try
            {
                string userName = $"{this.Name} {this.Surname}";
                this.UserID = 0;
                this.Email = null;
                this.Password = null;
                this.Name = null;
                this.Surname = null;
                this.UserTypeId = UserTypeId.ADMIN;
                this.UserState = false;

                return new UserResponse
                {
                    Success = true,
                    Message = $"Admin logout successful: {userName}",
                    User = null
                };
            }
            catch (Exception ex)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = $"An error occurred during admin logout: {ex.Message}",
                    User = this
                };
            }
        }

        public UserResponse FreezerAccount(int targetUserId)
        {
            try
            {
                using (var connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    // Önce mevcut state'i oku
                    string selectQuery = "SELECT UserState FROM Users WHERE UserID = @UserID";
                    using (var selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@UserID", targetUserId);
                        var currentStateObj = selectCommand.ExecuteScalar();
                        if (currentStateObj == null)
                        {
                            return new UserResponse { Success = false, Message = "User not found.", User = null };
                        }
                        bool currentState = Convert.ToBoolean(currentStateObj);
                        bool newState = !currentState;
                        // State'i tersine çevir
                        string updateQuery = "UPDATE Users SET UserState = @NewState WHERE UserID = @UserID";
                        using (var updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@NewState", newState);
                            updateCommand.Parameters.AddWithValue("@UserID", targetUserId);
                            int result = updateCommand.ExecuteNonQuery();
                            if (result > 0)
                            {
                                string msg = newState ? "User activated successfully." : "User deactivated successfully.";
                                return new UserResponse { Success = true, Message = msg, User = null };
                            }
                            else
                            {
                                return new UserResponse { Success = false, Message = "No user updated.", User = null };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new UserResponse { Success = false, Message = $"Error: {ex.Message}", User = null };
            }
        }

        // Kullanıcıyı doğrudan istenen state'e ayarlayan overload
        public UserResponse FreezerAccount(int targetUserId, bool newState)
        {
            try
            {
                using (var connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Users SET UserState = @NewState WHERE UserID = @UserID";
                    using (var updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@NewState", newState);
                        updateCommand.Parameters.AddWithValue("@UserID", targetUserId);
                        int result = updateCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            string msg = newState ? "User activated successfully." : "User deactivated successfully.";
                            return new UserResponse { Success = true, Message = msg, User = null };
                        }
                        else
                        {
                            return new UserResponse { Success = false, Message = "No user updated.", User = null };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new UserResponse { Success = false, Message = $"Error: {ex.Message}", User = null };
            }
        }

        public ListingResponse ActivateListing(int listingId)
        {
            return SetListingState(listingId, true);
        }

        public ListingResponse DeactivateListing(int listingId)
        {
            return SetListingState(listingId, false);
        }

        private ListingResponse SetListingState(int listingId, bool state)
        {
            try
            {
                using (var connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Listings SET ListingState = @State WHERE ListingID = @ListingID";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", state);
                        command.Parameters.AddWithValue("@ListingID", listingId);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return new ListingResponse { Success = true, Message = state ? "Listing activated." : "Listing deactivated.", Listing = null };
                        }
                        else
                        {
                            return new ListingResponse { Success = false, Message = "No listing updated.", Listing = null };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ListingResponse { Success = false, Message = $"Error: {ex.Message}", Listing = null };
            }
        }

        public ReservationResponse ApproveReservation(int reservationId)
        {
            return SetReservationState(reservationId, true);
        }

        public ReservationResponse CancelReservation(int reservationId)
        {
            return SetReservationState(reservationId, false);
        }

        private ReservationResponse SetReservationState(int reservationId, bool state)
        {
            try
            {
                using (var connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Reservations SET ReservationState = @State WHERE ReservationID = @ReservationID";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@State", state);
                        command.Parameters.AddWithValue("@ReservationID", reservationId);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return new ReservationResponse { Success = true, Message = state ? "Reservation approved." : "Reservation cancelled.", Reservation = null };
                        }
                        else
                        {
                            return new ReservationResponse { Success = false, Message = "No reservation updated.", Reservation = null };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ReservationResponse { Success = false, Message = $"Error: {ex.Message}", Reservation = null };
            }
        }

        // Diğer metodlar buraya eklenecek...
    }
} 