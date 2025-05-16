using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthManagmentSystem.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public decimal DoctorId { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public decimal PatientId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled, etc.

        // Navigation properties
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }
}
