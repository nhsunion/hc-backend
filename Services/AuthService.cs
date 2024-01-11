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

    public AuthService(IConfiguration configuration, AppDbcontext database)
    {
        _config = configuration;
        _db = database;
    }

    public string GenerateTokenPatient(Patient patient)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
            new Claim(ClaimTypes.Name, patient.Username),
            new Claim(ClaimTypes.Email, patient.Email) // User can login with email
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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
