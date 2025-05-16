using Microsoft.AspNetCore.Mvc;
using HealthManagmentSystem.Models;
using HealthManagmentSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace HealthManagmentSystem.Controllers
{
    public class ManagerController : Controller
    {
        private readonly HealthDbContext _context;

        public ManagerController(HealthDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        // =====================
        // APPOINTMENT SECTION
        // =====================
        [HttpGet]
        public IActionResult Appointment(int? doctorId, int? patientId, DateTime? startDate, DateTime? endDate, string status)
        {
            ViewBag.Doctors = new SelectList(_context.Doctor, "ID", "Name");
            ViewBag.Patients = new SelectList(_context.Patient, "ID", "Name");

            var appointments = _context.Appointment
                .Where(a =>
                    (!doctorId.HasValue || a.DoctorId == doctorId) &&
                    (!patientId.HasValue || a.PatientId == patientId) &&
                    (!startDate.HasValue || a.AppointmentDate >= startDate) &&
                    (!endDate.HasValue || a.AppointmentDate <= endDate) &&
                    (string.IsNullOrEmpty(status) || a.Status == status)
                )
                .ToList();

            return View(appointments);
        }

        [HttpGet]
        public IActionResult CreateAppointment()
        {
            ViewBag.Doctors = new SelectList(_context.Doctor, "ID", "Name");
            ViewBag.Patients = new SelectList(_context.Patient, "ID", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Appointment.Add(appointment);
                _context.SaveChanges();
                return RedirectToAction("Appointment");
            }

            ViewBag.Doctors = new SelectList(_context.Doctor, "ID", "Name", appointment.DoctorId);
            ViewBag.Patients = new SelectList(_context.Patient, "ID", "Name", appointment.PatientId);
            return View(appointment);
        }

        [HttpGet]
        public IActionResult EditAppointment(int id)
        {
            var appointment = _context.Appointment.Find(id);
            if (appointment == null) return NotFound();

            ViewBag.Doctors = new SelectList(_context.Doctor, "ID", "Name", appointment.DoctorId);
            ViewBag.Patients = new SelectList(_context.Patient, "ID", "Name", appointment.PatientId);

            return View(appointment);
        }

        [HttpPost]
        public IActionResult EditAppointment(Appointment updated)
        {
            if (ModelState.IsValid)
            {
                _context.Appointment.Update(updated);
                _context.SaveChanges();
                return RedirectToAction("Appointment");
            }

            ViewBag.Doctors = new SelectList(_context.Doctor, "ID", "Name", updated.DoctorId);
            ViewBag.Patients = new SelectList(_context.Patient, "ID", "Name", updated.PatientId);
            return View(updated);
        }

        [HttpPost]
        public IActionResult CancelAppointment(int id)
        {
            var appointment = _context.Appointment.Find(id);
            if (appointment == null) return NotFound();

            appointment.Status = "Cancelled";
            _context.SaveChanges();

            return RedirectToAction("Appointment");
        }

        public IActionResult AppointmentDetails(int id)
        {
            var appointment = _context.Appointment
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // =====================
        // MANAGE DOCTORS & PATIENTS
        // =====================
        public IActionResult Users()
        {
            var model = new AdminUserViewModel
            {
                Doctors = _context.Doctor.ToList(),
                Patients = _context.Patient.ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(string role, Doctor doctor, Patient patient)
        {
            if (role == "Doctor")
            {
                _context.Doctor.Add(doctor);
            }
            else if (role == "Patient")
            {
                _context.Patient.Add(patient);
            }
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult EditUser(int id, string role)
        {
            if (role == "Doctor")
            {
                var doctor = _context.Doctor.FirstOrDefault(d => d.ID == id);
                if (doctor == null) return NotFound();
                return View("EditDoctor", doctor);
            }
            else if (role == "Patient")
            {
                var patient = _context.Patient.FirstOrDefault(p => p.ID == id);
                if (patient == null) return NotFound();
                return View("EditPatient", patient);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult EditUser(string role, Doctor doctor, Patient patient)
        {
            if (role == "Doctor")
            {
                _context.Doctor.Update(doctor);
            }
            else if (role == "Patient")
            {
                _context.Patient.Update(patient);
            }
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id, string role)
        {
            if (role == "Doctor")
            {
                var doctor = _context.Doctor.Find(id);
                if (doctor != null)
                {
                    _context.Doctor.Remove(doctor);
                }
            }
            else if (role == "Patient")
            {
                var patient = _context.Patient.Find(id);
                if (patient != null)
                {
                    _context.Patient.Remove(patient);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult Profile()
        {
            // Dummy logic - replace with your real logic
            var manager = _context.Manager.FirstOrDefault(); // You might need to filter by logged-in manager
            if (manager == null) return NotFound();

            return View(manager);
        }
    }
}
