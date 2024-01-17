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

        public AppointmentController(AppDbcontext database)
        {
            _db = database;
        }

        [HttpPost("appointment")]
        public async Task<ActionResult<Appointment>> Create([FromBody] AppointmentDTO appointmentDto)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentDTO appointmentDto)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.PatientId = appointmentDto.PatientId;
            appointment.ProviderId = appointmentDto.ProviderId;
            appointment.Date = appointmentDto.Date;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _db.Appointments.Remove(appointment);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}