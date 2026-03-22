using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using SWD_Project.Models.Entities;
using SWD_Project.Models.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWD_Project.Controllers
{
    [Authorize(Roles = "Mentee")]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var requests = await _context.Requests
                .Include(r => r.Mentee)
                .Include(r => r.Mentor)
                .Where(r => r.MenteeId == userId)
                .ToListAsync();

            return View(requests);
        }

        // CREATE
        public IActionResult Create()
        {
            ViewBag.Mentors = _context.Users.Where(u => u.Role == Role.Mentor).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title,Content,MentorId")] Request request)
        {
            ModelState.Remove("Mentee");
            ModelState.Remove("Mentor");

            if (ModelState.IsValid)
            {
                request.MenteeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                request.CreatedAt = DateTime.Now;
                request.Status = RequestStatus.Pending;

                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Mentors = _context.Users.Where(u => u.Role == Role.Mentor).ToList();
            return View(request);
        }

        // EDIT
        public async Task<IActionResult> Edit(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.Requests.FindAsync(id);

            if (request == null || request.MenteeId != userId) return NotFound();

            if (request.Status != RequestStatus.Pending)
                return RedirectToAction(nameof(Index));

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Request model)
        {
            ModelState.Remove("Mentee");
            ModelState.Remove("Mentor");

            if (id != model.Id) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.Requests.FindAsync(id);

            if (request == null || request.MenteeId != userId) return NotFound();

            if (request.Status != RequestStatus.Pending)
                return RedirectToAction(nameof(Index));

            request.Title = model.Title;
            request.Content = model.Content;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.Requests
                .Include(r => r.Mentee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (request == null || request.MenteeId != userId) return NotFound();

            if (request.Status != RequestStatus.Pending)
            {
                TempData["Error"] = "Bạn chỉ có thể xóa yêu cầu khi đang ở trạng thái Chờ duyệt (Pending).";
                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.Requests.FindAsync(id);

            if (request != null && request.MenteeId == userId)
            {
                if (request.Status != RequestStatus.Pending)
                {
                    TempData["Error"] = "Bạn chỉ có thể xóa yêu cầu khi đang ở trạng thái Chờ duyệt (Pending).";
                    return RedirectToAction(nameof(Index));
                }

                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // STATISTICS
        public async Task<IActionResult> Statistics()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var stats = await _context.Requests
                .Where(r => r.MenteeId == userId)
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();

            return View(stats);
        }
    }
}
