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
                                    .Where(g => g.Any(d => d.ProductId == pProductId))
                                    .Select(g => new TransactionDto
                                    {
                                        OrderId = (int)g.Key,
                                        ProductIds = g.Select(d => (int)d.ProductId).ToList()
                                    })
                                    .ToList();

            return transactions;
        }

        public async Task<(Order order, Customer customer, List<DetailOrder> details)> GetOrderByInternalCode(string pInternalCode)
        {
            var order = await _context.Order.FirstOrDefaultAsync(x => x.InternalCode == pInternalCode);
            var customer = await _context.Customer.FirstOrDefaultAsync(x => x.Id == order.CustomerId);
            var details = await _context.DetailOrder.Where(x => x.OrderId == order.Id)
                                                    .Include(x => x.Product).ThenInclude(x => x.Capacity)
                                                    .Include(x => x.Product).ThenInclude(x => x.Color)
                                                    .ToListAsync();
            return (order, customer, details);
        }
    }
}
