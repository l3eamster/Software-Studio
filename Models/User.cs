using System;
using System.ComponentModel.DataAnnotations;

namespace DonutzStudio.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBan { get; set; }
    }
    public class LoginForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
    }
}