using System.ComponentModel.DataAnnotations;


namespace HealthManagmentSystem.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        public string Name { get; set; } = null!; // Add null! to suppress warning

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}

    




