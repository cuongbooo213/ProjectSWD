using Microsoft.AspNetCore.Mvc;

namespace SWD_Project.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
