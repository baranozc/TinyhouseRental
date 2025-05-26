using System;
using System.Data.SqlClient;

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
                this.UserType = UserType.ADMIN;
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
                    Message = "An error occurred during admin logout",
                    User = this
                };
            }
        }

        // Diğer metodlar buraya eklenecek...
    }
} 