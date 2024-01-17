using hc_backend.Data;
using hc_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hc_backend.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AuthController : Controller
    {
        private readonly AppDbcontext _db;
        private readonly AuthService _authService;

        public AuthController(AppDbcontext database, AuthService authService)
        {
            _db = database;
            _authService = authService;
        }


        [HttpPost("register/patient")]
        public async Task<ActionResult> CreatePatient([FromBody] RegisterRequest registerRequest)
        {
            if (await _db.Patients.AnyAsync(p => p.Username == registerRequest.Username || p.Email == registerRequest.Email))
            {
                return BadRequest("Username or Email already exists");
            }

            var patient = new Patient
            {
                Name = registerRequest.FullName,
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            var PasswordHasher = new PasswordHasher<Patient>();
            patient.Password = PasswordHasher.HashPassword(patient, patient.Password);

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();

            return Ok(new { patient.Username, patient.Email });
        }

         [HttpPost("register/provider")]
        public async Task<ActionResult> CreateProvider([FromBody] RegisterRequest registerRequest)
        {
            if (await _db.Providers.AnyAsync(p => p.Username == registerRequest.Username || p.Email == registerRequest.Email))
            {
                return BadRequest("Username or Email already exists");
            }

            var provider = new Provider
            {
                Name = registerRequest.FullName,
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            var PasswordHasher = new PasswordHasher<Provider>();
            provider.Password = PasswordHasher.HashPassword(provider, provider.Password);

            _db.Providers.Add(provider);
            await _db.SaveChangesAsync();

            return Ok(new { provider.Username, provider.Email });
        }


        [HttpPost("login/patient")]
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

            var token = _authService.GenerateTokenPatient(patient);

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

         [HttpPost("login/provider")]
        public async Task<ActionResult<List<Patient>>> LoginProvider([FromBody] LoginRequest loginRequest)
        {
            var provider = await _db.Providers.FirstOrDefaultAsync(p => p.Username == loginRequest.Username);
            if (provider == null)
            {
                return BadRequest("Incorrect Username or Password");
            }

            var PasswordHasher = new PasswordHasher<Provider>();
            var result = PasswordHasher.VerifyHashedPassword(provider, provider.Password, loginRequest.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Incorrect Username or Password");
            }

            string? token = null;

            try
            {
                token = _authService.GenerateTokenProvider(provider);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during token generation: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

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
