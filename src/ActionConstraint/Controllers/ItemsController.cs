using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ActionConstraint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([RequiredFromQuery]string filter)
        {
            var items = new string[] { "item1", "item2", "item10", "item20" }.Where(x => x.Contains(filter));
            return items.ToArray();
        }
    }
}
