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

    public class ExternalLab
    {
        public string labName { get; set; }
        public string itemName { get; set; }
        public string labImage { get; set; }
        public int itemAmount { get; set; }
        public string link { get; set; }
    }

}