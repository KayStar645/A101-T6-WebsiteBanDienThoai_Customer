using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly SmartPhoneDbContext _context;

        public OrderRepository(SmartPhoneDbContext pcontext) : base(pcontext)
        {
            _context = pcontext;
        }

        public async Task<string> RangeInternalCode()
        {
            var internalCodes = await _context.Order.Select(x => x.InternalCode).ToListAsync();
            int max = 0;
            foreach (var internalCode in internalCodes)
            {
                int number = int.Parse(internalCode.Substring(9));
                max = Math.Max(max, number);
            }
            max++;
            string code = max.ToString();
            while(code.Length < 7)
            {
                code = "0" + code;
            }
            DateTime currentDate = DateTime.Now;

            return "HD" + currentDate.ToString("yyMMdd") + "-" + code;
        }

        public async Task<List<TransactionDto>> GetTransactions(int pProductId)
        {
            var transactions = _context.DetailOrder
                                .Include(x => x.Order)
                                .Where(x => x.Order.Type == Order.TYPE_RECEIVED)
                                .GroupBy(d => d.OrderId)
                                .Select(g => new TransactionDto
                                {
                                    Id = (int)g.Key,
                                    ProductIds = g.Select(d => (int)d.ProductId).ToList()
                                })
                                .ToList();

            return transactions;
        }
    }
}
