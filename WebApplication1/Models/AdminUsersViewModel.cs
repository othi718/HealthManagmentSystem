namespace HealthManagmentSystem.Models
{
    public class AdminUsersViewModel
    {
        public List<Admin> Admins { get; set; } = null!;
        public List<Doctor> Doctors { get; set; } = null!;
        public List<Manager> Managers { get; set; } = null!;
        public List<Patient> Patients { get; set; } = null!;
    }
}