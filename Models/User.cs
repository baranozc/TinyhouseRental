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

        public abstract UserResponse Login(string email, string password);
        public abstract UserResponse Logout();
    }
} 