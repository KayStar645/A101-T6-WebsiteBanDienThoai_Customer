using Application.Interfaces.Services;
using Domain.DTOs;
using Domain.Requests.Products;
using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly IRecommendPurchasingTogetherService _recommendService;

        public StoreController(IProductService productService, ICategoryService categoryService, 
            IOrderService orderService, IRecommendPurchasingTogetherService recommendService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
            _recommendService = recommendService;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesResult = await _categoryService.GetAll();
            ViewData["categories"] = categoriesResult.Data;

            var listProductRequest = new ListProductRequest
            {
                Page = 1,
                PageSize = 4,
            };

            foreach (var category in categoriesResult.Data)
            {
                listProductRequest.CategoryId = category.Id;
                var products = await _productService.GetAll(listProductRequest);

                ViewData[category.Id.ToString()] = products.Data;
            }

            return View("_Home");
        }

        public async Task<IActionResult> Home()
        {
            var categoriesResult = await _categoryService.GetAll();
            ViewData["categories"] = categoriesResult.Data;

            var listProductRequest = new ListProductRequest
            {
                Page = 1,
                PageSize = 4,
            };

            foreach (var category in categoriesResult.Data)
            {
                listProductRequest.CategoryId = category.Id;
                var products = await _productService.GetAll(listProductRequest);

                ViewData[category.Id.ToString()] = products.Data;
            }

            return PartialView("_Home");
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var categoriesResult = await _categoryService.GetAll();
            ViewData["categories"] = categoriesResult.Data;

            var result = await _productService.GetDetail(id);
            ViewData["product"] = result.Data;

            var itemsets = await _recommendService.Get(id);
            // Lấy ds sp tương ứng
            var recommends = await _productService.GetProductForRecommend(itemsets, id);
            ViewData["recommends"] = recommends;

            return PartialView("_ProductDetail");
        }

        public async Task<IActionResult> Category(int id)
        {
            var categoriesResult = await _categoryService.GetAll();
            ViewData["categories"] = categoriesResult.Data;

            var listProductRequest = new ListProductRequest
            {
                Page = 1,
                CategoryId = id
            };
            var products = await _productService.GetAll(listProductRequest);

            ViewData["products"] = products.Data;

            return PartialView("_ProductByCategory");
        }

        public async Task<IActionResult> Cart()
        {
            var categoriesResult = await _categoryService.GetAll();
            ViewData["categories"] = categoriesResult.Data;

            var result = await _orderService.GetCart();
            ViewData["detailOrder"] = result.products;
            ViewData["sumPrice"] = result.sumPrice;

            return PartialView("_Cart");
        }

        [HttpGet]
        public async Task<IActionResult> Order(string? pInternalCode = "")
        {
            var categoriesResult = await _categoryService.GetAll();
            ViewData["categories"] = categoriesResult.Data;

            var result = await _orderService.GetOrderByInternalCode(pInternalCode);

            ViewData["order"] = result.order;
            ViewData["customer"] = result.customer;
            ViewData["details"] = result.details;
            if(result.order != null)
            {
                ViewData["type"] = Domain.Entities.Order.GetTypeMapping(result.order.Type).FirstOrDefault().typename;
            }    

            return PartialView("_Order");
        }
    }
}