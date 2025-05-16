namespace HealthManagmentSystem.Models.ViewModel
{
    public class AdminUserViewModel
    {
        public List<Admin> Admins { get; set; } = null!;
        public List<Doctor> Doctors { get; set; } = null!;
        public List<Manager> Managers { get; set; } = null!;
        public List<Patient> Patients { get; set; } = null!;
    }

    public class UserListViewModel<T>
    {
        public List<T> Users { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
