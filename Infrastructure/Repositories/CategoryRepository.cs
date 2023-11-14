using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly SmartPhoneDbContext _context;

        public CategoryRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }
    }
}
