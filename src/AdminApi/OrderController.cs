using Microsoft.AspNetCore.Mvc;
using ParallelPipelines;

namespace AdminApi
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
            return "Admin Order API. " + _orderService.GetOrders();
        }
    }
}
