using Application.Interfaces.Repositories.Common;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPromotionRepository : IBaseRepository<Promotion>
    {
        Task<List<Promotion>> GetByProductId(int pProductId);
    }
}
