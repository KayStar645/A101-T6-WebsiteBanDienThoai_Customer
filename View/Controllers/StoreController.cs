﻿using Application.Interfaces.Services;
using Domain.Requests.Products;
using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;

        public StoreController(IProductService productService, ICategoryService categoryService, 
            IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
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

            var detailOrder = await _orderService.GetCart();
            ViewData["detailOrder"] = detailOrder;

            return PartialView("_Cart");
        }

        public async Task<IActionResult> Order()
        {
            var categoriesResult = await _categoryService.GetAll();
            ViewData["categories"] = categoriesResult.Data;

            return PartialView("_Order");
        }
    }
}