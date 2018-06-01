using Microsoft.AspNetCore.Mvc;

namespace MvcPlugin
{
    public class ExternalHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
