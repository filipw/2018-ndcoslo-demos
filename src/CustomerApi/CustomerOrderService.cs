using ParallelPipelines;

namespace CustomerApi
{
    public class CustomerOrderService : IOrderService
    {
        public string GetOrders()
        {
            return "Only showing the orders for a customer";
        }
    }
}
