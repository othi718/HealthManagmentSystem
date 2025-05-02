using System.ComponentModel.DataAnnotations;

public class Login
{
    [Key]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;  // Initialize with empty string

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "Patient"; // Default role

    // Constructor for registration
    public Login() { } // Required for EF Core

    public Login(string email, string passwordHash, string role)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Role = role ?? "Patient"; // Default value if null
    }
}