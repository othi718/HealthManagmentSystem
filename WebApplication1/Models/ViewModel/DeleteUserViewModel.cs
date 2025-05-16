namespace HealthManagmentSystem.Models.ViewModel
{
    public class DeleteUserViewModel
    {
        public string Id { get; set; } = null!; // User ID (optional, depending on your needs)
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!; // "Admn", "Doctor", "Manager", or "Patient"

        // Additional user details you might want to display
        // For doctors
      //  public DateTime? RegistrationDate { get; set; } =null!; // For all users
       

        // For displaying related records that will be affected
        public int RelatedRecordsCount { get; set; }
        public string RelatedRecordsDescription { get; set; } = null!;
    }
}