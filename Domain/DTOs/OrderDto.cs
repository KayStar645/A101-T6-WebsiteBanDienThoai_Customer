using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTOs
{
    public class OrderDto
    {
        public string? InternalCode { get; set; }

        public long? SumPrice { get; set; }

        public Customer? Customer { get; set; }

        public List<DetailOrderDto>? Details { get; set; }
    }
}
