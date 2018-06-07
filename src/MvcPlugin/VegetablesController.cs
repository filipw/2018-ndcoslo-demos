using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MvcPlugin
{
    [Route("api/[controller]")]
    public class VegetablesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "🥕", "🌽", "🌶" };
        }
    }
}
