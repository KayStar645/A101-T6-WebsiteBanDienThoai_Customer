using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Product : Entity
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public string? Images { get; set; }

        public int? Quantity { get; set; }

        public long? Price { get; set; }

        [NotMapped]
        public long? NewPrice { get; set; }

        public int? CategoryId { get; set; }

        public int? ColorId { get; set; }

        public int? CapacityId { get; set; }

        public Promotion? Category { get; set; }

        public Color? Color { get; set; }

        public Capacity? Capacity { get; set; }

        public List<PromotionProduct> PromotionProducts { get; set; }

        public List<ProductParameters> ProductParameters { get; set; }
    }
}
