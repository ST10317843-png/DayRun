using Microsoft.AspNetCore.Mvc;

namespace DayRun.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
