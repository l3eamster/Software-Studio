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
        public string Color { get; set; }
        public string ItemImage { get; set; }
        public string LabImage { get; set; }
    }
}