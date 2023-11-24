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

        public async Task<bool> ReduceNumberAsync(int pProductId, int pNumber)
        {
            var product = await _context.Product.FindAsync(pProductId);
            if(product != null)
            {
                if(product.Quantity >= pNumber) 
                {
                    product.Quantity -= pNumber;
                    _context.Product.Update(product);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
