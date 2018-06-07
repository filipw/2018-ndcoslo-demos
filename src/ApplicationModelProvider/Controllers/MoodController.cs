using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationModelProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodController : ControllerBase
    {
        [HttpPost("happy")]
        public ActionResult<string> Post(RequestDto input, IHappyService svc)
        {
            return svc.BeHappy(input.Name);
        }

        [HttpPost("sad")]
        public ActionResult<string> Post(RequestDto input, ISadService svc)
        {
            return svc.BeSad(input.Name);
        }
    }

    public class RequestDto
    {
        public string Name { get; set; }
    }
}
