using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using SWD_Project.Models.Entities;

namespace SWD_Project.Controllers
{
    public class MenteeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenteeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<IActionResult> ListRequests()
        {
            var requests = await _context.Requests
                .Include(r => r.Mentee)
                .Include(r => r.Mentor)
                .ToListAsync();

            return View(requests);
        }

        // CREATE
        public IActionResult CreateRequest()
        {
            ViewBag.Mentors = _context.Users.Where(u => u.Role == Role.Mentor).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([Bind("Title,Content,MentorId")] Request request)
        {
            ModelState.Remove("Mentee");
            ModelState.Remove("Mentor");

            if (ModelState.IsValid)
            {
                request.MenteeId = 1; // demo
                request.CreatedAt = DateTime.Now;
                request.Status = RequestStatus.Pending;

                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListRequests));
            }

            ViewBag.Mentors = _context.Users.Where(u => u.Role == Role.Mentor).ToList();
            return View(request);
        }

        // UPDATE
        public async Task<IActionResult> UpdateRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null) return NotFound();

            if (request.Status != RequestStatus.Pending)
            {
                return RedirectToAction(nameof(ListRequests));
            }

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRequest(int id, [Bind("Id,Title,Content")] Request model)
        {
            ModelState.Remove("Mentee");
            ModelState.Remove("Mentor");

            if (id != model.Id) return NotFound();

            var request = await _context.Requests.FindAsync(id);
            if (request == null) return NotFound();

            if (request.Status != RequestStatus.Pending)
            {
                return RedirectToAction(nameof(ListRequests));
            }

            request.Title = model.Title;
            request.Content = model.Content;
            // request.Status is not modifiable by mentee

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListRequests));
        }

        // DELETE
        public async Task<IActionResult> DeleteRequest(int? id)
        {
            if (id == null) return NotFound();

            var request = await _context.Requests
                .Include(r => r.Mentee)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (request == null) return NotFound();

            if (request.Status != RequestStatus.Pending)
            {
                TempData["Error"] = "Bạn chỉ có thể xóa yêu cầu khi đang ở trạng thái Chờ duyệt (Pending).";
                return RedirectToAction(nameof(ListRequests));
            }

            return View(request);
        }

        [HttpPost, ActionName("DeleteRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                if (request.Status != RequestStatus.Pending)
                {
                    TempData["Error"] = "Bạn chỉ có thể xóa yêu cầu khi đang ở trạng thái Chờ duyệt (Pending).";
                    return RedirectToAction(nameof(ListRequests));
                }

                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ListRequests));
        }

        // STATISTIC
        public async Task<IActionResult> StatisticRequest()
        {
            var stats = await _context.Requests
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();

            return View(stats);
        }
        // MENTOR CV
        public async Task<IActionResult> ListMentorCVs(int? categoryId, int? skillId)
        {
            var query = _context.MentorCVs
                .Include(m => m.Mentor)
                .Include(m => m.Skills)
                    .ThenInclude(s => s.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(cv => cv.Skills.Any(s => s.CategoryId == categoryId.Value));
            }

            if (skillId.HasValue)
            {
                query = query.Where(cv => cv.Skills.Any(s => s.Id == skillId.Value));
            }

            var cvs = await query.ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            
            // Lọc danh sách Skill dựa theo CategoryId (nếu user chọn Category trước) 
            if (categoryId.HasValue)
            {
                ViewBag.Skills = await _context.Skills.Where(s => s.CategoryId == categoryId.Value).ToListAsync();
            }
            else
            {
                ViewBag.Skills = await _context.Skills.ToListAsync();
            }

            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedSkill = skillId;

            return View(cvs);
        }
    }
}
