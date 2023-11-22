using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class DetailOrderRepository : BaseRepository<DetailOrder>, IDetailOrderRepository
    {
        private readonly SmartPhoneDbContext _context;

        public DetailOrderRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }
    }
}