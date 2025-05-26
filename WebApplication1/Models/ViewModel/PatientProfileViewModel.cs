

    namespace HealthManagmentSystem.Models.ViewModel
    {
        public class PatientProfileViewModel
        {
        public Patient Patient { get; set; } = null!;
        public List<Appointment> Appointments { get; set; } = null!;
        public List<MedicalRecord> MedicalRecords { get; set; } = null!;
        }
    }


