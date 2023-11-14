using Application.Interfaces.Services;
using Domain.Requests.Products;
using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;

        private readonly IProductService _productService;

        private readonly ICategoryService _categoryService;

        public StoreController(ILogger<StoreController> logger, IProductService productService, ICategoryService categoryService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
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

                ViewData[category.Id.ToString()] = categoriesResult.Data;
            }

            return PartialView("_Layout");
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
    }
}