using Microsoft.AspNetCore.Mvc;

namespace SWD_Project.Controllers
{
    public class ChatbotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
