using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthManagmentSystem.Models
{
    public class MedicalRecord
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public decimal DoctorId { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public decimal PatientId { get; set; }

        [ForeignKey("Appointment")]
        public int? AppointmentId { get; set; }  // Optional link to an appointment

        [Required]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        public string Diagnosis { get; set; } = string.Empty;

        public string Treatment { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        // Navigation properties
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
