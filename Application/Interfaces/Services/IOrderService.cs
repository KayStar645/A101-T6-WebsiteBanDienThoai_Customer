using Domain.DTOs;
using Domain.Requests.Orders;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task AddProductToCart(int pProductId);

        Task AddProductsToCart(string pProductsId);

        Task UpdateQuantityProductInCart(int pProductId, int pQuantity);

        Task<(List<DetailOrderDto> products, long sumPrice)> GetCart();

        Task Create(CreateOrderRequest pRequest);
    }
}
