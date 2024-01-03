using hc_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace hc_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get the environment name
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Add the environment-specific appsettings file to the configuration
            builder.Configuration.AddJsonFile($"appsettings.{envName}.json", optional: true);

            builder.Services.AddControllers();

            // Get the connection string from the configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbcontext>(options => options.UseNpgsql(connectionString));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}