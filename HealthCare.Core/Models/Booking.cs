using System;
namespace HealthCare.Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string PatientName { get; set; }
        public string Service { get; set; }
    }

}

