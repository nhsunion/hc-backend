using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hc_backend.Data;
using hc_backend.Models;
using Microsoft.IdentityModel.Tokens;

public class AuthService
{
    private readonly IConfiguration _config;
    private readonly AppDbcontext _db;
    private readonly SymmetricSecurityKey _jwtKey;

    private const int MinimumKeyLengthBytes = 32;

    public AuthService(IConfiguration configuration, AppDbcontext database)
    {
        _config = configuration;
        _db = database;

        var jwtKeyString = _config["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKeyString) || jwtKeyString.Length < MinimumKeyLengthBytes)
        {
            // Handle the situation where the key is null, empty, or insufficient in length
            throw new InvalidOperationException("JWT key is not configured or is insufficient in length.");
        }

        _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyString));
    }

    public string GenerateTokenPatient(Patient patient)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
            new Claim(ClaimTypes.Name, patient.Username),
            new Claim(ClaimTypes.Email, patient.Email) // User can login with email
        };

        var creds = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateTokenProvider(Provider provider)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, provider.Id.ToString()),
            new Claim(ClaimTypes.Name, provider.Username),
            new Claim(ClaimTypes.Email, provider.Email)
        };

        var creds = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
