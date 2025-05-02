using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthManagmentSystem.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly HealthDbContext _context;

    public AdminController(HealthDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Users()
    {
        var model = new AdminUsersViewModel
        {
            Admins = await _context.Admin.ToListAsync(),
            Doctors = await _context.Doctor.ToListAsync(),
            Managers = await _context.Manager.ToListAsync(),
            Patients = await _context.Patient.ToListAsync()
        };
        return View(model);
    }

    public IActionResult AddUser()
    {
        return View(new Registration());
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(Registration model)
    {
        if (ModelState.IsValid)
        {
            // Check if email exists
            if (await _context.Login.AnyAsync(l => l.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(model);
            }

            // Create user based on role
            switch (model.Role)
            {
                case "Admin":
                    var admin = new Admin
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Role = model.Role
                    };
                    _context.Admin.Add(admin);
                    break;

                case "Doctor":
                    var doctor = new Doctor
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = model.Password,
                        ID = await GetNextDoctorId()
                    };
                    _context.Doctor.Add(doctor);
                    break;

                case "Manager":
                    var manager = new Manager
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = model.Password,
                        ID = await GetNextManagerId()
                    };
                    _context.Manager.Add(manager);
                    break;

                case "Patient":
                    var patient = new Patient
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = model.Password,
                        ID = await GetNextPatientId()
                    };
                    _context.Patient.Add(patient);
                    break;
            }

            // Add to Login table
            _context.Login.Add(new Login
            {
                Email = model.Email,
                PasswordHash = model.Password,
                Role = model.Role
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }
        return View(model);
    }

    private async Task<decimal> GetNextDoctorId()
    {
        var lastId = await _context.Doctor.MaxAsync(d => (decimal?)d.ID) ?? 0;
        return lastId + 1;
    }

    private async Task<decimal> GetNextManagerId()
    {
        var lastId = await _context.Manager.MaxAsync(m => (decimal?)m.ID) ?? 0;
        return lastId + 1;
    }

    private async Task<decimal> GetNextPatientId()
    {
        var lastId = await _context.Patient.MaxAsync(p => (decimal?)p.ID) ?? 0;
        return lastId + 1;
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string email, string role)
    {
        // Remove from role-specific table
        switch (role)
        {
            case "Admin":
                var admin = await _context.Admin.FirstOrDefaultAsync(a => a.Email == email);
                if (admin != null) _context.Admin.Remove(admin);
                break;

            case "Doctor":
                var doctor = await _context.Doctor.FirstOrDefaultAsync(d => d.Email == email);
                if (doctor != null) _context.Doctor.Remove(doctor);
                break;

            case "Manager":
                var manager = await _context.Manager.FirstOrDefaultAsync(m => m.Email == email);
                if (manager != null) _context.Manager.Remove(manager);
                break;

            case "Patient":
                var patient = await _context.Patient.FirstOrDefaultAsync(p => p.Email == email);
                if (patient != null) _context.Patient.Remove(patient);
                break;
        }

        // Remove from Login table
        var login = await _context.Login.FirstOrDefaultAsync(l => l.Email == email);
        if (login != null) _context.Login.Remove(login);

        await _context.SaveChangesAsync();
        return RedirectToAction("Users");
    }
}