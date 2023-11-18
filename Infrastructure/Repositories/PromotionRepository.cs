using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
    {
        private readonly SmartPhoneDbContext _context;

        public PromotionRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }

        public async Task<List<Promotion>> GetByProductId(int pProductId)
        {
            var result = await _context.PromotionProduct
                                .Include(x => x.Promotion)
                                .Where(x => x.ProductId == pProductId && x.IsDeleted == false)
                                .Select(x => x.Promotion)
                                .Where(x => x.Status == "A" && x.IsDeleted == false)
                                .ToListAsync();
            return result;
        }
    }
}
