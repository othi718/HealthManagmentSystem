using System.ComponentModel.DataAnnotations;

namespace HealthManagmentSystem.Models
{
    public class Patient
    {

        public string? Name { get; set; }
        [Key]
        public decimal? ID { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string? PasswordHash { get; set; }
        public string Role { get; set; } = "Patient"; // Default value

    }
}