using Microsoft.AspNetCore.Mvc;

namespace ParallelPipelines.CustomerApi
{
    [Route("[controller]")]
    public class DataController : CustomerBaseController
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
