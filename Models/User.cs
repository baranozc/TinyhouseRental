using System;

namespace MyProject.Models
{
    public enum UserType
    {
        CLIENT,
        ADMIN
    }

    public abstract class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserType UserType { get; set; }
        public bool UserState { get; set; }

        public abstract UserResponse Login(string email, string password);
        public abstract UserResponse Logout();
    }
} 