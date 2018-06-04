using Microsoft.AspNetCore.Mvc;
using ParallelPipelines;

namespace CustomerApi
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
            return "I'm Customer Data Controller. " + _resourceService.SayHi();
        }
    }
}
