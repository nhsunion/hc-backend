using System.ComponentModel.DataAnnotations;

public class AppointmentDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }
    public int? PatientId { get; set; }
    public string PatientName { get; set; }
    [Required(ErrorMessage = "Provider is required")]
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
}