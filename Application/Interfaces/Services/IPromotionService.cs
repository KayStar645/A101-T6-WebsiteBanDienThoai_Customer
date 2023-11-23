using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IPromotionService
    {
        Task<(Promotion? apply, long oldPrice, long newPrice)> ApplyPromotionForProduct(int pProductId);
    }
}
