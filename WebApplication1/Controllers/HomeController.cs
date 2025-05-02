using Microsoft.AspNetCore.Mvc;
using HealthManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace HealthManagmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly HealthDbContext _context;

        public HomeController(HealthDbContext context)
        {
            _context = context;
        }

        // 1. Home Page
        public IActionResult Index()
        {
            return View();
        }

        // 2. Registration (GET)
        public IActionResult Registration(string role)
        {
            ViewBag.Role = role ?? "Patient"; // Default if not specified
            return View();
        }

        // 3. Registration (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(Registration model)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = await _context.Registration.FirstOrDefaultAsync(x => x.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(model);
                }

                // Hash password (simple hash for now - you can improve later with real hashing)
                model.PasswordHash = model.Password; // Hashing logic should be improved, but for now storing directly
                _context.Registration.Add(model);

                // Also add to Login table
                var login = new Login
                {
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    Role = model.Role
                };
                _context.Login.Add(login);

                await _context.SaveChangesAsync();

                ViewBag.Role = model.Role;
                return RedirectToAction("RegistrationSuccess", new { role = model.Role });
            }

            return View(model);
        }

        // 4. Registration Success Page
        public IActionResult RegistrationSuccess(string role)
        {
            ViewBag.Role = role ?? "Patient";
            return View();
        }

        // 5. Login (GET)
        public IActionResult Login()
        {
            return View(new Models.ViewModel.LoginViewModel());
        }

        // 6. Login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Models.ViewModel.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Login.FirstOrDefaultAsync(x => x.Email == model.Email && x.PasswordHash == model.Password);
                if (user != null)
                {
                    // Validate email and role before creating claims
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        ModelState.AddModelError("", "User email is missing");
                        return View(model);
                    }

                    if (string.IsNullOrEmpty(user.Role))
                    {
                        ModelState.AddModelError("", "User role is missing");
                        return View(model);
                    }

                    // Create claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Redirect based on role
                    switch (user.Role)
                    {
                        case "Admin":
                            return RedirectToAction("Dashboard", "Admin");
                        case "Manager":
                            return RedirectToAction("Dashboard", "Manager");
                        case "Doctor":
                            return RedirectToAction("Dashboard", "Doctor");
                        case "Patient":
                            return RedirectToAction("Dashboard", "Patient");
                        default:
                            return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid email or password.";
                }
            }
            return View(model);
        }
        // 7. Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
