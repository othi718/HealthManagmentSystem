using HealthManagmentSystem.Models;
using HealthManagmentSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace HealthManagmentSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly HealthDbContext _context;

        public PatientController(HealthDbContext context)
        {
            _context = context;
        }

        // ✅ Dashboard after login
        public IActionResult Index()
        {
            return View(); // This will load Views/Patient/Index.cshtml (Dashboard view)
        }

        // ✅ View Doctor List
        public async Task<IActionResult> DoctorList()
        {
            var doctors = await _context.Doctor
                .Where(d => d.Role == "Doctor")
                .ToListAsync();

            return View(doctors); // Make sure Views/Patient/DoctorList.cshtml exists
        }

        // ✅ View Medical Records
        public async Task<IActionResult> MedicalRecords()
        {
            decimal patientId = GetLoggedInPatientId();

            var records = await _context.MedicalRecord
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId)
                .ToListAsync();

            return View(records); // Make sure Views/Patient/MedicalRecords.cshtml exists
        }

        // ✅ Appointment Dashboard (booking + history)
        public async Task<IActionResult> Appointment()
        {
            decimal patientId = GetLoggedInPatientId();

            var doctors = await _context.Doctor
                .Where(d => d.Role == "Doctor")
                .ToListAsync();

            var appointments = await _context.Appointment
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();

            var medicalRecords = await _context.MedicalRecord
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId)
                .ToListAsync();

            var model = new PatientAppointmentViewModel
            {
                Doctors = doctors,
                Appointment = new Appointment { AppointmentDate = DateTime.Now.AddDays(1) },
                Appointments = appointments,
                MedicalRecords = medicalRecords
            };

            return View(model); // Views/Patient/Appointment.cshtml
        }

        // ✅ Make appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.PatientId = GetLoggedInPatientId();
                appointment.Status = "Scheduled";

                _context.Appointment.Add(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Appointment");
            }

            decimal patientId = GetLoggedInPatientId();

            // Reload form with model data
            var doctors = await _context.Doctor
                .Where(d => d.Role == "Doctor")
                .ToListAsync();

            var appointments = await _context.Appointment
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();

            var medicalRecords = await _context.MedicalRecord
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId)
                .ToListAsync();

            var model = new PatientAppointmentViewModel
            {
                Doctors = doctors,
                Appointment = appointment,
                Appointments = appointments,
                MedicalRecords = medicalRecords
            };

            return View("Appointment", model);
        }

        // ✅ Cancel appointment
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null && appointment.PatientId == GetLoggedInPatientId())
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Appointment");
        }

        // Simulated logged-in patient ID
        private decimal GetLoggedInPatientId()
        {
            return 1; // Replace with real user ID from login/session
        }
    }
}
