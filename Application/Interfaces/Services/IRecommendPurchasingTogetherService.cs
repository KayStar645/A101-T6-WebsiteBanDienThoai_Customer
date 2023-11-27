using Domain.DTOs;

namespace Application.Interfaces.Services
{
    public interface IRecommendPurchasingTogetherService
    {
        Task<List<ItemsetResultDto>> Get(int pProductId);
    }
}
