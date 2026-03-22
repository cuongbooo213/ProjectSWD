using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using SWD_Project.Models.Entities;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SWD_Project.Controllers
{
    [Authorize(Roles = "Mentor")]
    public class MentorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MentorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> MyCV()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var cv = await _context.MentorCVs
                .Include(m => m.Skills)
                .FirstOrDefaultAsync(m => m.MentorId == userId);

            // Ensure we have an empty object to bind if null
            if (cv == null)
            {
                cv = new MentorCV { MentorId = userId };
            }

            ViewBag.AllSkills = await _context.Skills.Where(s => s.IsActive).ToListAsync();
            return View(cv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyCV(MentorCV model, int[] selectedSkills)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Prevent spoofing
            if (model.MentorId != userId)
            {
                return Forbid();
            }

            // Remove unneeded validations
            ModelState.Remove("Mentor");
            ModelState.Remove("Skills");

            if (!ModelState.IsValid)
            {
                ViewBag.AllSkills = await _context.Skills.Where(s => s.IsActive).ToListAsync();
                return View(model);
            }

            var existingCv = await _context.MentorCVs
                .Include(m => m.Skills)
                .FirstOrDefaultAsync(m => m.MentorId == userId);

            if (existingCv == null)
            {
                // Create new
                model.IsApproved = false;
                model.Skills = new List<Skill>();

                if (selectedSkills != null && selectedSkills.Length > 0)
                {
                    model.Skills = await _context.Skills
                        .Where(s => selectedSkills.Contains(s.Id))
                        .ToListAsync();
                }

                _context.MentorCVs.Add(model);
                TempData["Success"] = "Your CV has been created and is pending approval by the Admin.";
            }
            else
            {
                // Update existing
                existingCv.Bio = model.Bio;
                existingCv.ExperienceYears = model.ExperienceYears;
                existingCv.IsApproved = false; // Must be re-approved
                
                existingCv.Skills.Clear();
                if (selectedSkills != null && selectedSkills.Length > 0)
                {
                    var skillsToAdd = await _context.Skills
                        .Where(s => selectedSkills.Contains(s.Id))
                        .ToListAsync();
                    
                    foreach (var skill in skillsToAdd)
                    {
                        existingCv.Skills.Add(skill);
                    }
                }

                _context.MentorCVs.Update(existingCv);
                TempData["Success"] = "Your CV has been updated and is pending re-approval by the Admin.";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("MyCV");
        }
    }
}
