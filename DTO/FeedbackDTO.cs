using System.ComponentModel.DataAnnotations;

namespace hc_backend.DTO
{
    public class FeedbackDTO
    {
        [Required]
        public string Message { get; set; }

        public int? PatientId { get; set; }
    }
}
