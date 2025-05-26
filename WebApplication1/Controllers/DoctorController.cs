using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthManagmentSystem.Models;

public class DoctorController : Controller
{
    private readonly HealthDbContext _context;

    public DoctorController(HealthDbContext context)
    {
        _context = context;
    }

    // Loads the Doctor Dashboard (Views/Doctor/Index.cshtml)
    public IActionResult Index()
    {
        return View();
    }

    // Loads the doctor's scheduled appointments (Views/Doctor/Appointment.cshtml)
    public async Task<IActionResult> Appointment()
    {
        var doctorEmail = User.Identity?.Name;

        if (string.IsNullOrEmpty(doctorEmail))
            return Unauthorized();

        // Find the doctor based on their email (logged-in identity)
        var doctor = await _context.Doctor
            .FirstOrDefaultAsync(d => d.Email == doctorEmail);

        if (doctor == null)
            return NotFound();

        // Get appointments assigned to this doctor
        var appointments = await _context.Appointment
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctor.ID)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        return View(appointments);
    }


    // Optional: Add Profile view routing if you use a profile view for doctor
    public async Task<IActionResult> Profile()
    {
        var doctorEmail = User.Identity?.Name;

        if (string.IsNullOrEmpty(doctorEmail))
            return Unauthorized();

        var doctor = await _context.Doctor
            .FirstOrDefaultAsync(d => d.Email == doctorEmail);

        if (doctor == null)
            return NotFound();

        return View(doctor); // This should match Views/Doctor/Profile.cshtml
    }
}
