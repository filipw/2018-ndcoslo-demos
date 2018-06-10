using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePackages.Controllers
{
    [Route("api/[controller]")]
    public class HelloController
    {
        [HttpGet]
        public ActionResult<string> GetHello()
        {
            return "hello";
        }
    }
}
