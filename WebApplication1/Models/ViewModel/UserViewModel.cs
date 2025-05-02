using System.ComponentModel.DataAnnotations;

namespace HealthManagmentSystem.Models
{
  
        // Models/UserViewModel.cs
namespace HealthManagmentSystem.Models
    {
        public class UserViewModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Email { get; set; } = null!;
            [Required]
            public string Password { get; set; } = null!;
            [Required]
            public string Role { get; set; } = null!;// "Admin", "Doctor", "Manager", or "Patient"

            // For display purposes
            public string FullName => $"{FirstName} {LastName}";
        }
    }
}

