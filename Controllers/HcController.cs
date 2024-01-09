using hc_backend.Data;
using hc_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hc_backend.Controllers
{
    [ApiController]
    [Route("/api")]
    public class HcController : Controller
    {
        private readonly AppDbcontext _db;
        private readonly AuthService _authService;

        public HcController(AppDbcontext database, AuthService authService)
        {
            _db = database;
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<ActionResult> CreatePatient(Patient patient)
        {
            if (await _db.Patients.AnyAsync(p => p.Username == patient.Username || p.Email == patient.Email))
            {
                return BadRequest("Username or Email already exists");
            }
            var PasswordHasher = new PasswordHasher<Patient>();
            patient.Password = PasswordHasher.HashPassword(patient, patient.Password);

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();

            return Ok(patient);
        }


        [HttpGet("login")]
        public async Task<ActionResult<List<Patient>>> LoginPatient()
        {
            var patients = await _db.Patients.ToListAsync();
            if (patients.Count == 0)
            {
                return NotFound();
            }
            return Ok(patients);
        }

    }

}
