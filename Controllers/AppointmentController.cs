using hc_backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hc_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly AppDbcontext _db;

        public AppointmentController(AppDbcontext db)
        {
            _db = db;
        }

        [HttpPost("appointment")]
        public async Task<ActionResult<Appointment>> CreateAvailableAppointment([FromBody] AppointmentDTO appointmentDto)
        {
            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                ProviderId = appointmentDto.ProviderId,
                Date = appointmentDto.Date
            };

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAvailableAppointments()
        {
            var appointments = await _db.Appointments
                            .Include(a => a.Provider)
                            .Include(a => a.Patient)
                            .Where(a => a.PatientId == null)
                            .ToListAsync();

            List<AppointmentDTO> appointmentDTOS = new();
            foreach (var appointment in appointments)
            {
                appointmentDTOS.Add(new AppointmentDTO
                {
                    Id = appointment.Id,
                    Date = appointment.Date,
                    PatientId = null,
                    PatientName = null,
                    ProviderId = appointment.ProviderId,
                    ProviderName = appointment.Provider.Name
                });
            }

            return appointmentDTOS;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDTO>> GetById(int id)
        {
            var appointment = await _db.Appointments
                .Include(a => a.Provider)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDto = new AppointmentDTO
            {
                Id = appointment.Id,
                Date = appointment.Date,
                ProviderId = appointment.ProviderId,
                ProviderName = appointment.Provider.Name, 
                // Provider must always exist
                // Patient cannot book an appointment without a provider
            };

            if (appointment.Patient != null)
            {
                appointmentDto.PatientId = appointment.Patient.Id;
                appointmentDto.PatientName = appointment.Patient.Name;
            }

            return appointmentDto;
        }
    }
}