using System.ComponentModel.DataAnnotations;

namespace HealthManagmentSystem.Models
{
    public class Manager
    {



        public string? Name { get; set; }
        [Key]
        public decimal? ID { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }
        public string Role { get; set; } = "Manager"; // Default value
    }
   
}


