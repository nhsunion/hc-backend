using System.ComponentModel.DataAnnotations;

namespace hc_backend.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
    }
}
