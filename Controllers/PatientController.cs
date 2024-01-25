using hc_backend.Data;
using hc_backend.DTO;
using hc_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hc_backend.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbcontext _db;
        public PatientController(AppDbcontext db)
        {
            _db = db;
        }

        [HttpGet("patients")]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAllPatients()
        {
            var patients = await _db.Patients.ToListAsync();

            List<PatientDTO> patientDTOS = new();
            foreach (var patient in patients)
            {
                patientDTOS.Add(new PatientDTO
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Email = patient.Email,
                    Phone = patient.Phone,
                    Address = patient.Address,
                    Username = patient.Username
                });
            }

            return patientDTOS;
        }
    }
}
