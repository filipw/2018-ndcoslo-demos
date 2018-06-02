using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MvcPlugin
{
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "hello from external plugin! Eat more vegetables" };
        }
    }
}
