using hc_backend.Data;
using hc_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hc_backend.Controllers
{
    [ApiController]
    [Route("/api")]
    public class HcController : Controller
    {
        private readonly AppDbcontext _db;

        public HcController(AppDbcontext database)
        {
            _db = database;
        }

        [HttpPost("register")] 
        public async Task<ActionResult> CreatePatient(Patient patient)
        {
            var patients = await _db.Patients.ToListAsync();

            if (patients.Any(p => p.Username == patient.Username))
            {
                return BadRequest("");
            }
            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();
            return Ok(patient);
        }

        [HttpGet("login")] 
        public async Task<ActionResult<List<Patient>>> GetPatient()
        {
            var patients = await _db.Patients.ToListAsync();
            if (patients.Count == 0) // ToListAsync() will never return null
            {
                return NotFound();
            }
            return Ok(patients);
        }

    }

}
