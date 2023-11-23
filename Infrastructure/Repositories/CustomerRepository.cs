using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Repositories.Common;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly SmartPhoneDbContext _context;

        public CustomerRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }
    }
}
