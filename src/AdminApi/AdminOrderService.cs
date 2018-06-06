using ParallelPipelines;

namespace AdminApi
{
    public class AdminOrderService : IOrderService
    {
        public string GetOrders()
        {
            return "Admin is almighty, showing all orders";
        }
    }
}
