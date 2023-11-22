using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly SmartPhoneDbContext _context;

        public OrderRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }
    }
}
