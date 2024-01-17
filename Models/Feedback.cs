using System.ComponentModel.DataAnnotations;

namespace hc_backend.Models
{
    public class Feedback
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }

        // Patient not required, if a patient wants to leave anonymous feedback.
        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}
