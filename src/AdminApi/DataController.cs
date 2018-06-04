using Microsoft.AspNetCore.Mvc;
using ParallelPipelines;

namespace AdminApi
{
    [Route("[controller]")]
    public class DataController : Controller
    {
        private IHiService _resourceService;

        public DataController(IHiService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet]
        public string Get()
        {
            return "I'm Admin Data Controller. " + _resourceService.SayHi();
        }
    }
}
