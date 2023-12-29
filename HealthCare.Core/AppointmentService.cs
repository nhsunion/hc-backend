using System;
namespace HealthCare.Core
{
	public class AppointmentService
	{
        private List<AppointmentDetails> appointments;

        public AppointmentService()
        {
            // mock data
            appointments = new List<AppointmentDetails>
            {
                new AppointmentDetails { Id = "1", PatientId = "100", Details = "Appointment with Dr. Smith" },
                new AppointmentDetails { Id = "2", PatientId = "101", Details = "Appointment with Dr. Johnson" }
            };
        }

        public AppointmentDetails GetAppointmentDetails(string appointmentId)
        {
            return appointments.FirstOrDefault(a => a.Id == appointmentId);
        }


        public class AppointmentDetails
        {
            public string Id { get; set; }
            public string PatientId { get; set; }
            public string Details { get; set; }
        }
    }
}

