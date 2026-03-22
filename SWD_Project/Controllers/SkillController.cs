using Microsoft.AspNetCore.Mvc;

namespace SWD_Project.Controllers
{
    public class SkillController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("ListSkills", "Admin");
        }
    }
}
