using Microsoft.AspNetCore.Mvc;

namespace MvcPlugin
{
    public class GoodAdviceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
