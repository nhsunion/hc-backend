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

            return Ok(new {patient.Username, patient.Email});
        }


        [HttpPost("login")]
        public async Task<ActionResult<List<Patient>>> LoginPatient([FromBody] LoginRequest loginRequest)
        {
            var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Username == loginRequest.Username);
            if (patient == null)
            {
                return BadRequest("Incorrect Username or Password");
            }

            var PasswordHasher = new PasswordHasher<Patient>();
            var result = PasswordHasher.VerifyHashedPassword(patient, patient.Password, loginRequest.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Incorrect Username or Password");
            }

            var token = _authService.GenerateToken(patient);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // frontend and backend are on different domains
                Expires = DateTime.UtcNow.AddDays(7)
                // TODO: Implement anti-forgery token to prevent CSRF attacks
            });

            return Ok();
        }
    }

}
