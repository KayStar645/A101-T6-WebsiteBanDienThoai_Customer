using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Application.Responses;
using Domain.DTOs;
using Domain.Entities;
using Domain.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        private readonly IPromotionRepository _promotionRepo;

        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo, IPromotionRepository promotionRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _promotionRepo = promotionRepo;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<List<ProductDto>>> GetAll(ListProductRequest pRequest)
        {
            var query = _productRepo.GetAllInclude();

            query = query.Where(x => x.CategoryId == pRequest.CategoryId && x.IsDeleted == false);

            int totalCount = await query.CountAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            query = query.GroupBy(x => x.Name).Select(group => new Product
            {
                Name = group.Key,
                Id = group.FirstOrDefault().Id,
                InternalCode = group.FirstOrDefault().InternalCode,
                Images = group.FirstOrDefault().Images,
                Quantity = group.FirstOrDefault().Quantity,
                Price = group.FirstOrDefault().Price,
            });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Áp dụng phân trang
            query = query.Skip((int)((pRequest.Page - 1) * pRequest.PageSize))
                         .Take((int)pRequest.PageSize);

            var products = await query.ToListAsync();

            foreach(var product in products)
            {
                // Lấy tất cả CTKM đang áp dụng cho sp này và chọn cái giảm giá cao nhất
                var promotions = await _promotionRepo.GetByProductId(product.Id);

                Promotion applyPromotion = null;
                long discountMax = 0;

                foreach (var promotion in promotions)
                {
                    if((promotion.Start <= DateTime.Now && DateTime.Now <= promotion.End) == false)
                    {
                        continue;
                    }    

                    if(promotion.Type == "D")
                    {
                        long? discount = promotion.Discount > promotion.PercentMax * product.Price * 0.01 ?
                                                              (long)(promotion.PercentMax * product.Price * 0.01) :
                                                              promotion.Discount;
                        if(discountMax < discount)
                        {
                            discountMax = (long)discount;
                            product.NewPrice = product.Price - discountMax;
                            applyPromotion = promotion;
                        }    

                    } 
                    else if(promotion.Type == "P")
                    {
                        long? discount = promotion.DiscountMax < promotion.Percent * product.Price * 0.01 ?
                                                                 promotion.DiscountMax : 
                                                                 (long)(promotion.Percent * product.Price * 0.01);
                        if (discountMax < discount)
                        {
                            product.NewPrice = product.Price - discountMax;
                            discountMax = (long)discount;
                            applyPromotion = promotion;
                        }
                    }    
                }
                if(applyPromotion != null)
                {
                    product.PromotionProducts = new List<PromotionProduct>{
                        new PromotionProduct
                        {
                            ProductId = product.Id,
                            PromotionId = applyPromotion.Id,
                            Promotion = applyPromotion,
                        }
                    };
                }  
            }

            var mapProducts = _mapper.Map<List<ProductDto>>(products);

            return PaginatedResult<List<ProductDto>>.Success(
                mapProducts, totalCount, pRequest.Page,
                pRequest.PageSize);
        }

    }
}
