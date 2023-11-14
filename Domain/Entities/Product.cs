using Domain.Common;

namespace Domain.Entities
{
    public class Product : Entity
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public string? Images { get; set; }

        public int? Quantity { get; set; }

        public long? Price { get; set; }

        public int? CategoryId { get; set; }

        public int? ColorId { get; set; }

        public int? CapacityId { get; set; }

        public Category? Category { get; set; }

        //public Color? Color { get; set; }

        //public Capacity? Capacity { get; set; }
    }
}
