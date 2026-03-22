using Microsoft.AspNetCore.Mvc;

namespace SWD_Project.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("ListCategories", "Admin");
        }
    }
}
