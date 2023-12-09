using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Requests.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly IHttpContextAccessor _httpContext;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository orderRepository, IDetailOrderRepository detailOrderRepository,
           ICustomerRepository customerRepository, IMapper mapper, IPromotionService promotionService,
           IHttpContextAccessor httpContextAccessor, IProductRepository productRepo)
        {
            _orderRepo = orderRepository;
            _detailOrderRepo = detailOrderRepository;
            _customerRepo = customerRepository;
            _mapper = mapper;
            _promotionService = promotionService;
            _httpContext = httpContextAccessor;
            _productRepo = productRepo;
        }

        public async Task Create(CreateOrderRequest pRequest)
        {
            // Ràng buộc dữ liệu
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Tạo khách hàng
                    var mapCustomer = _mapper.Map<Customer>(pRequest.Customer);
                    var resultCustomer = await _customerRepo.AddAsync(mapCustomer);

                    // Tạo đơn hàng
                    var createOrder = new Order
                    {
                        InternalCode = await _orderRepo.RangeInternalCode(),
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
                        detailOrder.DiscountPrice = result.oldPrice - result.newPrice;
                        detailOrder.SumPrice = result.newPrice * detailOrder.Quantity;

                        // Cập nhật giá lại cho đơn hàng
                        sumPrice[0] += detailOrder.Price * detailOrder.Quantity;
                        sumPrice[1] += detailOrder.DiscountPrice;
                        sumPrice[2] += detailOrder.SumPrice;    

                        detailOrder.OrderId = resultOrder.Id;
                        var detail = _mapper.Map<DetailOrder>(detailOrder);

                        var resultDetail = await _detailOrderRepo.AddAsync(detail);
                    }
                    createOrder.Price = sumPrice[0];
                    createOrder.DiscountPrice = sumPrice[1];
                    createOrder.SumPrice = sumPrice[2];

                    await _orderRepo.UpdateAsync(createOrder);

                    transaction.Complete();
                    _httpContext.HttpContext.Response.Cookies.Delete("products");
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                }
            }            
        }

        public async Task AddProductToCart(int pProductId)
        {
            var list = await GetCart();
            if(list.products.Any(x => x.ProductId == pProductId))
            {
                return;
            }    

            var product = await _productRepo.Query()
                        .Include(x => x.Capacity)
                        .Include(x=> x.Color)
                        .FirstOrDefaultAsync(x => x.Id == pProductId);

            var mapProduct = _mapper.Map<ProductDto>(product);
            // Lấy tất cả CTKM đang áp dụng cho sp này và chọn cái giảm giá cao nhất
            var result = await _promotionService.ApplyPromotionForProduct(mapProduct.Id);

            var detailOrder = new DetailOrderDto
            {
                Product = mapProduct,
                ProductId = pProductId,
                DiscountPrice = result.oldPrice - result.newPrice,
                Price = result.oldPrice,
                SumPrice = result.newPrice * 1,
                Quantity = 1,
            };


            var productsCookie = _httpContext.HttpContext.Request.Cookies["products"];

            List<DetailOrderDto> productsList;
            if (string.IsNullOrEmpty(productsCookie))
            {
                productsList = new List<DetailOrderDto>();
            }
            else
            {
                productsList = JsonConvert.DeserializeObject<List<DetailOrderDto>>(productsCookie);
            }

            productsList.Add(detailOrder);

            var productsJson = JsonConvert.SerializeObject(productsList);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
            };
            _httpContext.HttpContext.Response.Cookies.Append("products", productsJson, cookieOptions);
        }

        public async Task AddProductsToCart(string pProductsId)
        {
            var list = await GetCart();
            List<DetailOrderDto> productsList = new List<DetailOrderDto>();

            string[] ids = pProductsId.Split(",");
            foreach (string id in ids)
            {
                if (list.products.Any(x => x.ProductId == int.Parse(id)))
                {
                    continue;
                }

                var product = await _productRepo.Query()
                            .Include(x => x.Capacity)
                            .Include(x => x.Color)
                            .FirstOrDefaultAsync(x => x.Id == int.Parse(id));

                var mapProduct = _mapper.Map<ProductDto>(product);
                // Lấy tất cả CTKM đang áp dụng cho sp này và chọn cái giảm giá cao nhất
                var result = await _promotionService.ApplyPromotionForProduct(mapProduct.Id);

                var detailOrder = new DetailOrderDto
                {
                    Product = mapProduct,
                    ProductId = int.Parse(id),
                    DiscountPrice = result.oldPrice - result.newPrice,
                    Price = result.oldPrice,
                    SumPrice = result.newPrice * 1,
                    Quantity = 1,
                };


                var productsCookie = _httpContext.HttpContext.Request.Cookies["products"];

                if (string.IsNullOrEmpty(productsCookie) == false)
                {
                    productsList = JsonConvert.DeserializeObject<List<DetailOrderDto>>(productsCookie);
                }

                productsList.Add(detailOrder);
            }    

            var productsJson = JsonConvert.SerializeObject(productsList);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
            };
            _httpContext.HttpContext.Response.Cookies.Append("products", productsJson, cookieOptions);
        }

        public async Task UpdateQuantityProductInCart(int pProductId, int pQuantity)
        {
            var list = await GetCart();
            var product = list.products.FirstOrDefault(x => x.ProductId == pProductId);
            if (product != null)
            {
                product.SumPrice = (product.SumPrice / product.Quantity) * pQuantity;
                product.Quantity = pQuantity;

            }
            var productsJson = JsonConvert.SerializeObject(list.products);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
            };
            _httpContext.HttpContext.Response.Cookies.Append("products", productsJson, cookieOptions);
        }

        public async Task<(List<DetailOrderDto> products, long sumPrice)> GetCart()
        {

            var productsCookie = _httpContext.HttpContext.Request.Cookies["products"];

            List<DetailOrderDto> products;
            if (string.IsNullOrEmpty(productsCookie))
            {
                products = new List<DetailOrderDto>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<DetailOrderDto>>(productsCookie);
            }
            long sumPrice = products.Sum(x => x.SumPrice);
            return (products, sumPrice);
        }

        public async Task<(OrderDto order, CustomerDto customer, List<DetailOrderDto> details)> GetOrderByInternalCode(string pInternalCode)
        {
            try
            {
                var result = await _orderRepo.GetOrderByInternalCode(pInternalCode);
                var orderDto = _mapper.Map<OrderDto>(result.order);
                var customerDto = _mapper.Map<CustomerDto>(result.customer);
                var detailsDto = _mapper.Map<List<DetailOrderDto>>(result.details);

                return (orderDto, customerDto, detailsDto);
            }
            catch
            {
                return (null, null, null);
            }
        }

    }
}
