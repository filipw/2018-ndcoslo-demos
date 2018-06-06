using Microsoft.AspNetCore.Mvc;
using ParallelPipelines;

namespace CustomerApi
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public string Get()
        {
            return "Customer Order API. " + _orderService.GetOrders();
        }
    }
}
