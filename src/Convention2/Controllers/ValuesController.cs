using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;

namespace Convention1.Controllers
{
    public class ValuesController : ControllerBase
    {
        public ActionResult<IEnumerable<string>> GetAll()
        {
            return new string[] { "value1", "value2" };
        }

        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        public void Post([FromBody] string value)
        {
        }

        public void Put(int id, [FromBody] string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
