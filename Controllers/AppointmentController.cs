using hc_backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace hc_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly AppDbcontext _db;

        public AppointmentController(AppDbcontext database)
        {
            _db = database;
        }

        [HttpPost]
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
        public async Task<ActionResult<Appointment>> GetById(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
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

            _db.Appointments.Update(appointment);
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