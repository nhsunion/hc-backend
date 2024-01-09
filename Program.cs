using System.Text;
using hc_backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace hc_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["Jwt:Issuer"],
                       ValidAudience = builder.Configuration["Jwt:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                   };
               });

            builder.Services.AddScoped<AuthService>();

            // Get the environment name
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Add the environment-specific appsettings file to the configuration

            // $env:ASPNETCORE_ENVIRONMENT="Admin" (windows) after creating appsettings.Admin.json
            builder.Configuration.AddJsonFile($"appsettings.{envName}.json", optional: true);

            builder.Services.AddControllers();

            // Get the connection string from the configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbcontext>(options => options.UseNpgsql(connectionString));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // If the environmentName(launchSettings.json) is "Admin", enable Swagger 
            if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Admin")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}