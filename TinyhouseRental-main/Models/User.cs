using System;

namespace MyProject.Models
{
    public enum UserTypeId
    {
        ADMIN,
        CLIENT
    }

    public abstract class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserTypeId UserTypeId { get; set; }
        public bool UserState { get; set; }
        public string About { get; set; }

        public abstract UserResponse Login(string email, string password);
        public abstract UserResponse Logout();

        // Statik kullanıcı bulucu
        public static User FindByEmailAndPassword(string email, string password)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(MyProject.Config.DatabaseConfig.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
                using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool userState = Convert.ToBoolean(reader["UserState"]);
                            if (!userState)
                            {
                                throw new InactiveUserException();
                            }
                            var userTypeId = (int)reader["UserTypeId"];
                            if (userTypeId == (int)UserTypeId.CLIENT)
                            {
                                return new Client
                                {
                                    UserID = (int)reader["UserID"],
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Surname = reader["Surname"].ToString(),
                                    UserTypeId = UserTypeId.CLIENT,
                                    UserState = userState
                                };
                            }
                            else if (userTypeId == (int)UserTypeId.ADMIN)
                            {
                                return new Admin
                                {
                                    UserID = (int)reader["UserID"],
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Surname = reader["Surname"].ToString(),
                                    UserTypeId = UserTypeId.ADMIN,
                                    UserState = userState
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }

        public class InactiveUserException : Exception
        {
            public InactiveUserException() : base("User account is inactive.") { }
        }
    }
} 