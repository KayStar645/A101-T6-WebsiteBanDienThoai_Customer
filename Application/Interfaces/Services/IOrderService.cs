using Domain.DTOs;
using Domain.Requests.Orders;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task AddProductToCart(int pProductId);

        Task<List<DetailOrderDto>> GetCart();

        Task Create(CreateOrderRequest pRequest);
    }
}
