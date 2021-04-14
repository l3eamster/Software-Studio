using System;
using System.ComponentModel.DataAnnotations;

namespace DonutzStudio.Models
{
    public class Lab
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
        public int ItemCount { get; set; }
    }
}