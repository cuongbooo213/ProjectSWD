using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using System.Threading.Tasks;

namespace SWD_Project.Controllers
{
    public class MenteeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenteeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // MENTOR CV
        public async Task<IActionResult> ListMentorCVs(int? categoryId, int? skillId)
        {
            var query = _context.MentorCVs
                .Where(m => m.IsApproved)
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

            ViewBag.Categories = await _context.SkillCategories.ToListAsync();
            
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
