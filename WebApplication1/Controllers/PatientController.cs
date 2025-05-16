using HealthManagmentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace HealthManagmentSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly HealthDbContext _context;

        public PatientController(HealthDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            decimal patientId = GetLoggedInPatientId();

            var doctors = await _context.Doctor.ToListAsync();

            var appointments = await _context.Appointment
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();

            var model = new PatientIndexViewModel
            {
                Doctors = doctors,
                Appointments = appointments,
                NewAppointment = new Appointment { AppointmentDate = DateTime.Now.AddDays(1) }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.PatientId = GetLoggedInPatientId();
                appointment.Status = "Scheduled";

                _context.Appointment.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // If invalid, reload Index view with data
            var doctors = await _context.Doctor.ToListAsync();
            var appointments = await _context.Appointment
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == GetLoggedInPatientId())
                .ToListAsync();

            var model = new PatientIndexViewModel
            {
                Doctors = doctors,
                Appointments = appointments,
                NewAppointment = appointment
            };
            return View("Index", model);
        }

        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        private decimal GetLoggedInPatientId()
        {
            // TODO: Replace with real logged-in patient ID retrieval logic
            return 1;
        }
    }

    // ViewModel to hold data for Index view
    public class PatientIndexViewModel
    {
        public List<Doctor> Doctors { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
        public Appointment NewAppointment { get; set; } = new();
    }
}
