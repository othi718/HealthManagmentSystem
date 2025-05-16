using System.ComponentModel.DataAnnotations;
namespace HealthManagmentSystem.Models
{ 

public class Registration
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Key]
    [Required]
    [EmailAddress]
    public string Email { get; set; } 

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? PasswordHash { get; set; } // Nullable as it's set programmatically

    [Required]
    public string Role { get; set; } = "Patient"; // Default value
} 
}