using Microsoft.AspNetCore.Mvc;

namespace SWD_Project.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
