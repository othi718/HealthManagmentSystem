using HealthManagmentSystem.Models;
using System.Collections.Generic;


namespace HealthManagmentSystem.Models.ViewModel
{
    public class PatientAppointmentViewModel
    {
        public List<Doctor> Doctors { get; set; } = new();
        public Appointment Appointment { get; set; } = new();

        public List<Appointment> Appointments { get; set; } = new();
        public List<MedicalRecord> MedicalRecords { get; set; } = new();
    }

}
