using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Convention1.Controllers
{ 
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int versionNumber)
        {
            if (versionNumber == 1)
                return Ok(new string[] { "item1", "item2" });

            return Ok(new Item[] { new Item { Name = "item1" }, new Item { Name = "item2" } });
        }
    }

    public class Item
    {
        public string Name { get; set; }
    }
}
