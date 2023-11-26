using Domain.DTOs;

namespace Application.Interfaces.Services
{
    public interface IRecommendPurchasingTogetherService
    {
        Task<List<int>> Get(int pProductId);
    }
}
