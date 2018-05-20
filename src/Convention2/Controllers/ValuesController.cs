using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;

namespace Convention1.Controllers
{
    [Route("api/[controller]")]
    [UseWebApiActionConventions]
    [UseWebApiOverloading]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [Route("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [Route("{id}")]
        public void Delete(int id)
        {
        }
    }
}
