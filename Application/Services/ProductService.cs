using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Application.Responses;
using Domain.DTOs;
using Domain.Entities;
using Domain.Requests.Products;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        private readonly IMapper _mapper;

        private readonly IPromotionService _promotionService;

        public ProductService(IProductRepository productRepo, IMapper mapper, IPromotionService promotionService)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _promotionService = promotionService;
        }

        public async Task<PaginatedResult<List<ProductDto>>> GetAll(ListProductRequest pRequest)
        {
            var query = _productRepo.GetAllInclude();

            query = query.Where(x => x.CategoryId == pRequest.CategoryId);

            query = query.Include(x => x.Color);
            query = query.Include(x => x.Capacity);

            int totalCount = await query.CountAsync();
            // Áp dụng phân trang
            query = query.Skip((int)((pRequest.Page - 1) * pRequest.PageSize))
                         .Take((int)pRequest.PageSize);

            var products = await query.ToListAsync();

            foreach(var product in products)
            {
                // Lấy tất cả CTKM đang áp dụng cho sp này và chọn cái giảm giá cao nhất
                var result = await _promotionService.ApplyPromotionForProduct(product.Id);
                product.NewPrice = result.newPrice;

                if (result.apply != null)
                {
                    product.PromotionProducts = new List<PromotionProduct>{
                        new PromotionProduct
                        {
                            ProductId = product.Id,
                            PromotionId = result.apply.Id,
                            Promotion = result.apply,
                        }
                    };
                }
            }

            var mapProducts = _mapper.Map<List<ProductDto>>(products);

            return PaginatedResult<List<ProductDto>>.Success(
                mapProducts, totalCount, pRequest.Page,
                pRequest.PageSize);
        }

        public async Task<Result<ProductDto>> GetDetail(int pId)
        {
            var query = _productRepo.FindByCondition(x => x.Id == pId);

            query = query.Include(x => x.Color);
            query = query.Include(x => x.Capacity);
            query = query.Include(x => x.ProductParameters)
                            .ThenInclude(x =>x.DetailSpecifications)
                                .ThenInclude(x => x.Specifications);

            var product = await query.FirstOrDefaultAsync();

            // Lấy tất cả CTKM đang áp dụng cho sp này và chọn cái giảm giá cao nhất
            var result = await _promotionService.ApplyPromotionForProduct(product.Id);
            product.NewPrice = result.newPrice;

            if (result.apply != null)
            {
                product.PromotionProducts = new List<PromotionProduct>{
                    new PromotionProduct
                    {
                        ProductId = product.Id,
                        PromotionId = result.apply.Id,
                        Promotion = result.apply,
                    }
                };
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Result<ProductDto>.Success(productDto, (int)HttpStatusCode.OK);
        }
    }
}
