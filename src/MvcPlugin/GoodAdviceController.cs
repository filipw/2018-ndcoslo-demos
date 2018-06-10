using Microsoft.AspNetCore.Mvc;

namespace MvcPlugin
{
    public class GoodAdviceController : Controller
    {
        public IActionResult Index()
        {
            return View(new Advice
            {
                Title = "Vegetables",
                Text = "Eat more vegetables"
            });
        }
    }

    public class Advice
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
