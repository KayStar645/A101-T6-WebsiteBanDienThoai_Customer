using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Orders;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Application.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IDetailOrderRepository _detailOrderRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IMapper _mapper;
        private readonly IPromotionService _promotionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IOrderRepository orderRepository, IDetailOrderRepository detailOrderRepository,
           ICustomerRepository customerRepository, IMapper mapper, IPromotionService promotionService,
           IHttpContextAccessor httpContextAccessor)
        {
            _orderRepo = orderRepository;
            _detailOrderRepo = detailOrderRepository;
            _customerRepo = customerRepository;
            _mapper = mapper;
            _promotionService = promotionService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Giảm số lượng sản phẩm
        public async Task Create(CreateOrderRequest pRequest)
        {
            // Ràng buộc dữ liệu
            using (var transaction = new TransactionScope())
            {
                try
                {
                    // Tạo khách hàng
                    var mapCustomer = _mapper.Map<Customer>(pRequest.Customer);
                    var resultCustomer = await _customerRepo.AddAsync(mapCustomer);

                    // Tạo đơn hàng
                    var createOrder = new Order
                    {
                        InternalCode = "tự sinh",
                        OrderDate = DateTime.Now,
                        Type = Order.TYPE_ORDER,
                        CustomerId = resultCustomer.Id,
                        Price = 0,
                        DiscountPrice = 0,
                        SumPrice = 0,
                    };
                    var resultOrder = await _orderRepo.AddAsync(createOrder);

                    // Tạo chi tiết đơn hàng
                    long[] sumPrice = new long[] { 0, 0, 0 }; // Hiện tại, giảm, Tổng sau giảm
                    
                    // Duyệt qua chi tiết đơn hàng
                    foreach (var detailOrder in pRequest.Details)
                    {
                        // Lấy tất cả CTKM đang áp dụng cho sp này và chọn cái giảm giá cao nhất
                        var result = await _promotionService.ApplyPromotionForProduct(detailOrder.ProductId);

                        // Cập nhật giá lại cho chi tiết sp
                        detailOrder.Price = result.oldPrice;
                        detailOrder.DiscountPrice = result.newPrice - result.oldPrice;
                        detailOrder.SumPrice = result.newPrice * detailOrder.Quantity;

                        // Cập nhật giá lại cho đơn hàng
                        sumPrice[0] += detailOrder.Price;
                        sumPrice[1] += detailOrder.DiscountPrice;
                        sumPrice[2] += detailOrder.SumPrice;

                        // Giảm số lượng sản phẩm

                        detailOrder.OrderId = resultOrder.Id;
                        var detail = _mapper.Map<DetailOrder>(detailOrder);

                        var resultDetail = await _detailOrderRepo.AddAsync(detail);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                }
            }            
        }

        public async Task Cart()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                // HttpOnly, Secure, SameSite, ...
            };

            httpContext.Response.Cookies.Append("thuanpt", "thuanpt", cookieOptions);
        }
    }
}
