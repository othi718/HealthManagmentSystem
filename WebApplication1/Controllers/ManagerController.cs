using HealthManagmentSystem.Models;
using Microsoft.AspNetCore.Mvc;
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

        // Dashboard
        public IActionResult Index()
        {
            ViewBag.DoctorCount = _context.Doctor.Count();
            ViewBag.PatientCount = _context.Patient.Count();
            return View();
        }

        // View all doctors
        public IActionResult Doctors()
        {
            var doctors = _context.Doctor.ToList();
            return View(doctors);
        }

        // View all patients
        public IActionResult Patients()
        {
            var patients = _context.Patient.ToList();
            return View(patients);
        }

        // Add doctor (GET)
        public IActionResult AddDoctor()
        {
            return View();
        }

        // Add doctor (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDoctor(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.Role = "Doctor";
                _context.Doctor.Add(doctor);
                _context.SaveChanges();
                return RedirectToAction("Doctors");
            }
            return View(doctor);
        }

        // Delete doctor
        public IActionResult DeleteDoctor(decimal id)
        {
            var doctor = _context.Doctor.Find(id);
            if (doctor != null)
            {
                _context.Doctor.Remove(doctor);
                _context.SaveChanges();
            }
            return RedirectToAction("Doctors");
        }

        // Add patient (GET)
        public IActionResult AddPatient()
        {
            return View();
        }

        // Add patient (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.Role = "Patient";
                _context.Patient.Add(patient);
                _context.SaveChanges();
                return RedirectToAction("Patients");
            }
            return View(patient);
        }

        // Delete patient
        public IActionResult DeletePatient(decimal id)
        {
            var patient = _context.Patient.Find(id);
            if (patient != null)
            {
                _context.Patient.Remove(patient);
                _context.SaveChanges();
            }
            return RedirectToAction("Patients");
        }

        // Optional: View Manager Profile (future expansion)
        public IActionResult Profile()
        {
            // Get manager info from session or auth context
            // Placeholder example
            var manager = _context.Manager.FirstOrDefault();
            return View(manager);
        }
    }
}
