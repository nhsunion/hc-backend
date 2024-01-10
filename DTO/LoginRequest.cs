using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}