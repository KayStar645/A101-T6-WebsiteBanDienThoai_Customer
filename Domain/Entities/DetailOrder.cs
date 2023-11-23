using Domain.Common;

namespace Domain.Entities
{
    public class DetailOrder : Entity
    {
        public int? Quantity { get; set; }

        public long? DiscountPrice { get; set; }

        public long? Price { get; set; }

        public long? SumPrice { get; set; }

        public int? ProductId { get; set; }

        public int? OrderId { get; set; }

        public Product? Product { get; set; }

        public Order? Order { get; set; }
    }
}
