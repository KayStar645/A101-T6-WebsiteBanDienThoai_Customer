using Domain.Common;

namespace Domain.Entities
{
    public class PromotionProduct : Entity
    {

        public int ProductId { get; set; }

        public int PromotionId { get; set; }

        public Product Product { get; set; }

        public Promotion Promotion { get; set; }
    }
}
