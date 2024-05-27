using Microsoft.AspNetCore.Mvc;

namespace TaskManagerUI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
