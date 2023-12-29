using System;
using HealthCare.Core.Models;
namespace HealthCare.Core
{
    public class BookingService
    {
        private List<Booking> bookings = new List<Booking>();

        public BookingService()
        {
            // mock data
            bookings.Add(new Booking { Id = 1, Time = DateTime.Now.AddHours(2), PatientName = "John Doe", Service = "General Checkup" });
            bookings.Add(new Booking { Id = 2, Time = DateTime.Now.AddHours(4), PatientName = "Jane Smith", Service = "Vaccination" });
         
        }

        public IEnumerable<Booking> GetBookings()
        {
            return bookings;
        }

        public void AddBooking(Booking booking)
        {
            bookings.Add(booking);
        }
    }
}

