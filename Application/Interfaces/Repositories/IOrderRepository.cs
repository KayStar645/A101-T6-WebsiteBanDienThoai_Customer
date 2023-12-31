﻿using Application.Interfaces.Repositories.Common;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<string> RangeInternalCode();

        Task<List<TransactionDto>> GetTransactions(int pProductId);

        Task<(Order order, Customer customer, List<DetailOrder> details)> GetOrderByInternalCode(string pInternalCode);
    }
}
