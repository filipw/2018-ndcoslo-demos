using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationModelProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetController : ControllerBase
    {
        [HttpPost("hello")]
        public ActionResult<string> Post(RequestDto input, IHelloService svc)
        {
            return svc.SayHello() + " " + input.Name;
        }

        [HttpPost("bye")]
        public ActionResult<string> Post(RequestDto input, IGoodbyeService svc)
        {
            return svc.SayGoodbye() + " " + input.Name;
        }
    }

    public class RequestDto
    {
        public string Name { get; set; }
    }
}
