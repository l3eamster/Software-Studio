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
    }
}