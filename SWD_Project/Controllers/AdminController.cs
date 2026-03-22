using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWD_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== CV MANAGEMENT =====================

        [HttpGet]
        public async Task<IActionResult> ManageCVs()
        {
            var cvs = await _context.MentorCVs
                .Include(c => c.Mentor)
                .Include(c => c.Skills)
                .OrderBy(c => c.IsApproved)
                .ThenByDescending(c => c.Id)
                .ToListAsync();

            return View(cvs);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCV(int id, bool approve)
        {
            var cv = await _context.MentorCVs.FindAsync(id);
            if (cv != null)
            {
                cv.IsApproved = approve;
                await _context.SaveChangesAsync();
                TempData["Success"] = approve ? "CV Approved successfully." : "CV Rejected successfully.";
            }
            return RedirectToAction(nameof(ManageCVs));
        }

        // ===================== ACCOUNT MANAGEMENT =====================

        [HttpGet]
        public async Task<IActionResult> ListAccounts(string search = null, string role = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.Username.Contains(search) ||
                                         u.FullName.Contains(search) ||
                                         u.Email.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                if (System.Enum.TryParse<SWD_Project.Models.Enums.Role>(role, out var roleEnum))
                {
                    query = query.Where(u => u.Role == roleEnum);
                }
            }

            ViewBag.Search = search;
            ViewBag.Role = role;
            var users = await query.OrderBy(u => u.Role).ThenBy(u => u.Username).ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AccountDetails(int id)
        {
            var user = await _context.Users
                .Include(u => u.Requests)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            ViewBag.CV = await _context.MentorCVs
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.MentorId == id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> BlockAccount(int id, bool block)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsBlocked = block;
                await _context.SaveChangesAsync();
                TempData["Success"] = block ? "Account has been blocked." : "Account has been unblocked.";
            }
            return RedirectToAction(nameof(AccountDetails), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(int id, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                TempData["Error"] = "Password must be at least 6 characters.";
                return RedirectToAction(nameof(AccountDetails), new { id });
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.PasswordHash = HashPassword(newPassword);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Password has been reset successfully.";
            }
            return RedirectToAction(nameof(AccountDetails), new { id });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // ===================== SKILL CATEGORY MANAGEMENT =====================

        [HttpGet]
        public async Task<IActionResult> ListCategories(string search = null)
        {
            var query = _context.SkillCategories.Include(c => c.Skills).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Name.Contains(search) || c.Description.Contains(search));

            ViewBag.Search = search;
            var categories = await query.OrderBy(c => c.Name).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(SWD_Project.Models.Entities.SkillCategory model)
        {
            ModelState.Remove("Skills");
            if (!ModelState.IsValid) return View(model);

            _context.SkillCategories.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(ListCategories));
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _context.SkillCategories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(SWD_Project.Models.Entities.SkillCategory model)
        {
            ModelState.Remove("Skills");
            if (!ModelState.IsValid) return View(model);

            _context.SkillCategories.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(ListCategories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.SkillCategories
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            if (category.Skills != null && category.Skills.Any())
            {
                TempData["Error"] = "Cannot delete category that has skills. Remove all skills in this category first.";
                return RedirectToAction(nameof(ListCategories));
            }

            _context.SkillCategories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(ListCategories));
        }

        // ===================== SKILL MANAGEMENT =====================

        [HttpGet]
        public async Task<IActionResult> ListSkills(string search = null, int? categoryId = null)
        {
            var query = _context.Skills.Include(s => s.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(s => s.Name.Contains(search) || s.Description.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(s => s.CategoryId == categoryId.Value);

            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = await _context.SkillCategories.OrderBy(c => c.Name).ToListAsync();

            var skills = await query.OrderBy(s => s.Category.Name).ThenBy(s => s.Name).ToListAsync();
            return View(skills);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSkill()
        {
            ViewBag.Categories = await _context.SkillCategories.OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSkill(SWD_Project.Models.Entities.Skill model)
        {
            ModelState.Remove("Category");
            ModelState.Remove("MentorCVs");
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.SkillCategories.OrderBy(c => c.Name).ToListAsync();
                return View(model);
            }

            _context.Skills.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Skill created successfully.";
            return RedirectToAction(nameof(ListSkills));
        }

        [HttpGet]
        public async Task<IActionResult> EditSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();
            ViewBag.Categories = await _context.SkillCategories.OrderBy(c => c.Name).ToListAsync();
            return View(skill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSkill(SWD_Project.Models.Entities.Skill model)
        {
            ModelState.Remove("Category");
            ModelState.Remove("MentorCVs");
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.SkillCategories.OrderBy(c => c.Name).ToListAsync();
                return View(model);
            }

            _context.Skills.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Skill updated successfully.";
            return RedirectToAction(nameof(ListSkills));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Skill deleted successfully.";
            return RedirectToAction(nameof(ListSkills));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSkillActive(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                skill.IsActive = !skill.IsActive;
                await _context.SaveChangesAsync();
                TempData["Success"] = skill.IsActive ? "Skill activated." : "Skill deactivated.";
            }
            return RedirectToAction(nameof(ListSkills));
        }
    }
}
