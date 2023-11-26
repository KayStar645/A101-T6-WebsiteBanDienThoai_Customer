using Application.Interfaces.Services;
using Domain.DTOs;
using Domain.Requests.Orders;
using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;

        public OrderController(ICategoryService categoryService, IOrderService orderService)
        {
            _categoryService = categoryService;
            _orderService = orderService;
        }

        public async Task<IActionResult> AddProductToCart(int pProductId)
        {
            await _orderService.AddProductToCart(pProductId);

            return RedirectToAction("Cart", "Store");
        }

        public async Task<IActionResult> UpdateQuantityProductInCart(int pProductId, int pQuantity)
        {
            await _orderService.UpdateQuantityProductInCart(pProductId, pQuantity);

            return RedirectToAction("Cart", "Store");
        }

        public async Task<IActionResult> Create(string name, string address, string phone)
        {
            var customerDto = new CustomerDto
            {
                InternalCode = Guid.NewGuid().ToString(),
                Name = name,
                Phone = phone,
                Address = address,
            };

            var order = new CreateOrderRequest
            {
                Customer = customerDto,
                Details = (await _orderService.GetCart()).products
            };

            await _orderService.Create(order);
            return RedirectToAction("Order", "Store");
        }
    }
}
