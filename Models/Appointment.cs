using System.ComponentModel.DataAnnotations;
using hc_backend.Models;

public class Appointment
{
    [Required]
    public int Id { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public int? PatientId { get; set; }
    public Patient Patient { get; set; }
    [Required]
    public int ProviderId { get; set; }
    public Provider Provider { get; set; }
}