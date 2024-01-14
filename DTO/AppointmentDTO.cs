using System.ComponentModel.DataAnnotations;

public class AppointmentDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }
    [Required(ErrorMessage = "Patient is required")]
    public int PatientId { get; set; }
    [Required(ErrorMessage = "Provider is required")]
    public int ProviderId { get; set; }
}