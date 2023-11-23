using Domain.DTOs;

namespace Domain.Requests.Orders
{
    public class CreateOrderRequest
    {
        public CustomerDto? Customer { get; set; }

        public List<DetailOrderDto> Details { get; set; }
    }
}
