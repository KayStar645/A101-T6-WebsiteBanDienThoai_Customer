using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepo;

        private readonly IProductRepository _productRepo;

        private readonly IMapper _mapper;

        public PromotionService(IPromotionRepository promotionRepo, IProductRepository productRepository, IMapper mapper)
        {
            _promotionRepo = promotionRepo;
            _productRepo = productRepository;
            _mapper = mapper;
        }

        public async Task<(Promotion? apply, long oldPrice, long newPrice)> ApplyPromotionForProduct(int pProductId)
        {
            var product = await _productRepo.FirstOrDefaultAsync(x => x.Id == pProductId);
            long newPrice = product.Price ?? 0;

            var promotions = await _promotionRepo.GetByProductId(pProductId);
            Promotion applyPromotion = null;
            long discountMax = 0;

            foreach (var promotion in promotions)
            {
                if ((promotion.Start <= DateTime.Now && DateTime.Now <= promotion.End) == false)
                {
                    continue;
                }

                if (promotion.Type == "D")
                {
                    long? discount = promotion.Discount > promotion.PercentMax * product.Price * 0.01 ?
                                                          (long)(promotion.PercentMax * product.Price * 0.01) :
                                                          promotion.Discount;
                    if (discountMax < discount)
                    {
                        discountMax = (long)discount;
                        newPrice = (long)(product.Price - discountMax);
                        applyPromotion = promotion;
                    }

                }
                else if (promotion.Type == "P")
                {
                    long? discount = promotion.DiscountMax < promotion.Percent * product.Price * 0.01 ?
                                                             promotion.DiscountMax :
                                                             (long)(promotion.Percent * product.Price * 0.01);
                    if (discountMax < discount)
                    {
                        discountMax = (long)discount;
                        newPrice = (long)(product.Price - discountMax);
                        applyPromotion = promotion;
                    }
                }
            }

            return (applyPromotion, (long)product.Price, newPrice);
        }
    }
}
