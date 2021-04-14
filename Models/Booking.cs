using System;
using System.ComponentModel.DataAnnotations;

namespace DonutzStudio.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LabId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        // 0 = เช้า 9-12, 1 = บ่าย 13-16, 2 = ค่ำ 17-20
        public int Time { get; set; }
    }

    public class BookingForm
    {
        public int UserId { get; set; }
        public int LabId { get; set; }

        public class BookingData
        {
            // [DataType(DataType.Date)]
            public string Date { get; set; }
            public bool[] Booking { get; set; }
        }
        public BookingData[] BookingList { get; set; }
    }
}