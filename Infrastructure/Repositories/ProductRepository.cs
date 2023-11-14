using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Requests.Products;
using Domain.ViewModels;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly SmartPhoneDbContext _context;

        public ProductRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }

        // Thêm các truy vấn khác nếu có
        public async Task<List<ProductVM>> GetAllAsync(ListProductRequest pRequest)
        {
            var result = _context.Product
                .Include(x => x.Color)
                .Include(x => x.Capacity)
                .Where(x => x.CategoryId == pRequest.CategoryId)
                .GroupBy(x => x.Name)
                .Select(group => new
                {
                    Name = group.Key,
                    Products = group.ToList(),
                    Colors = group.Select(x => x.Color).ToList(),
                    Capacities = group.Select(x => x.Capacity).ToList(),
                })
                .ToList();


            return null;
        }
    }
}
