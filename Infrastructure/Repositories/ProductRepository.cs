using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly SmartPhoneDbContext _context;

        public ProductRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }
    }
}
