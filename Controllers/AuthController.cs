using System.Security.Claims;
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


        [HttpPost("login")]
        public async Task<ActionResult<UserRole>> Login([FromBody] LoginRequest loginRequest)
        {
            var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Username == loginRequest.Username);
            var provider = await _db.Providers.FirstOrDefaultAsync(p => p.Username == loginRequest.Username);

            if (patient == null && provider == null)
            {
                return BadRequest("Incorrect Username or Password");
            }

            PasswordVerificationResult result;
            string? token = null;
            string role;

            if (patient != null)
            {
                var PasswordHasher = new PasswordHasher<Patient>();
                result = PasswordHasher.VerifyHashedPassword(patient, patient.Password, loginRequest.Password);
                role = "patient";
            }
            else
            {
                var PasswordHasher = new PasswordHasher<Provider>();
                result = PasswordHasher.VerifyHashedPassword(provider, provider.Password, loginRequest.Password);
                role = "provider";
            }

            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Incorrect Username or Password");
            }

            try
            {
                if (role == "patient")
                {
                    token = _authService.GenerateTokenPatient(patient);
                }
                else
                {
                    token = _authService.GenerateTokenProvider(provider);
                }
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

            return Ok(new UserRole { Role = role, Username = loginRequest.Username });
        }

              [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("jwt"))
            {
                Response.Cookies.Append("jwt", "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1)
                });

                return Ok("Logged out successfully.");
            }

            return BadRequest("No active session found.");
        }

        [Authorize]
        [HttpGet("user/current")]
        public async Task<ActionResult<UserRole>> CurrentUser()
        {
            var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (username == null)
            {
                Console.WriteLine("User is null");
            }
            else
            {
                Console.WriteLine($"username: {username}");
            }
            var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Username == username);
            var provider = await _db.Providers.FirstOrDefaultAsync(p => p.Username == username);

            if (patient == null && provider == null)
            {
                return BadRequest("User not found");
            }

            string role;

            if (patient != null)
            {
                role = "patient";
            }
            else
            {
                role = "provider";
            }

            return Ok(new UserRole { Role = role, Username = username });
        }

     
    }

}
