using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using SWD_Project.Models.Entities;
using SWD_Project.Models.Enums;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWD_Project.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrWhiteSpace(username))
                ModelState.AddModelError("Username", "Username is required");

            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("Password", "Password is required");

            if (!ModelState.IsValid)
                return View();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                if (user.IsBlocked)
                {
                    ModelState.AddModelError(string.Empty, "Your account has been blocked. Please contact support.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("FullName", user.FullName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            string username,
            string fullName,
            string email,
            string password,
            string confirmPassword,
            Role role)
        {
            if (string.IsNullOrWhiteSpace(username))
                ModelState.AddModelError("Username", "Username is required");

            if (string.IsNullOrWhiteSpace(fullName))
                ModelState.AddModelError("FullName", "Full Name is required");

            if (string.IsNullOrWhiteSpace(email))
                ModelState.AddModelError("Email", "Email is required");

            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("Password", "Password is required");

            if (string.IsNullOrWhiteSpace(confirmPassword))
                ModelState.AddModelError("ConfirmPassword", "Confirm Password is required");
            else if (password != confirmPassword)
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");

            if (role != Role.Mentee && role != Role.Mentor)
                ModelState.AddModelError("Role", "Please select a valid role (Mentee or Mentor)");

            if (!ModelState.IsValid)
                return View();

            if (_context.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return View();
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return View();
            }

            var user = new User
            {
                Username = username,
                FullName = fullName,
                Email = email,
                Role = role,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful! You can now log in.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            if (hash == password) return true;
            return HashPassword(password) == hash;
        }
    }
}