using hc_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace hc_backend.Data
{
    public class AppDbcontext : DbContext
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options)
            : base(options)
        {
        }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Patient> Patients { get; set; }
    }
}
