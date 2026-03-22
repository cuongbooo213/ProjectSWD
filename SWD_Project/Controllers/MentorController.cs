using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using SWD_Project.Models.Entities;
using SWD_Project.Models.Enums;
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

        [HttpGet]
        public async Task<IActionResult> ListRequests()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var requests = await _context.Requests
                .Include(r => r.Mentee)
                .Where(r => r.MentorId == userId || (r.MentorId == null && r.Status == RequestStatus.Pending))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRequest(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction(nameof(ListRequests));
            }

            // Only Pending requests can be accepted
            if (request.Status != RequestStatus.Pending)
            {
                TempData["Error"] = "This request is no longer available.";
                return RedirectToAction(nameof(ListRequests));
            }

            // Can accept if it's open (MentorId null) OR directly assigned to this mentor
            if (request.MentorId == null || request.MentorId == userId)
            {
                request.MentorId = userId;
                request.Status = RequestStatus.Accepted;
                await _context.SaveChangesAsync();
                TempData["Success"] = "You have successfully accepted the request.";
            }
            else
            {
                TempData["Error"] = "You don't have permission to accept this request.";
            }

            return RedirectToAction(nameof(ListRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequest(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction(nameof(ListRequests));
            }

            // Can only reject Direct Requests that are currently Pending
            if (request.MentorId == userId && request.Status == RequestStatus.Pending)
            {
                request.Status = RequestStatus.Rejected;
                await _context.SaveChangesAsync();
                TempData["Success"] = "You have rejected the request.";
            }
            else
            {
                TempData["Error"] = "You cannot reject this request.";
            }

            return RedirectToAction(nameof(ListRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteRequest(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction(nameof(ListRequests));
            }

            // Can only complete Accepted requests assigned to this mentor
            if (request.MentorId == userId && request.Status == RequestStatus.Accepted)
            {
                request.Status = RequestStatus.Completed;
                await _context.SaveChangesAsync();
                TempData["Success"] = "You have marked the request as Completed.";
            }
            else
            {
                TempData["Error"] = "You cannot complete this request.";
            }

            return RedirectToAction(nameof(ListRequests));
        }
    }
}
