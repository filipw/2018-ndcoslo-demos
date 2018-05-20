using Microsoft.AspNetCore.Mvc;

namespace ParallelPipelines.DashboardApi
{
    [Route("[controller]")]
    public class DataController : DashboardBaseController
    {
        private IHiService _hiService;

        public DataController(IHiService hiService)
        {
            _hiService = hiService;
        }

        [HttpGet]
        public string Get()
        {
            return "I'm Dashboard Data Controller. " + _hiService.SayHi();
        }
    }
}
