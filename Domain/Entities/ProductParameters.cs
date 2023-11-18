using Domain.Common;

namespace Domain.Entities
{
    public class ProductParameters : Entity
    {
        public int? DetailSpecificationsId { get; set; }
        public int? ProductId { get; set; }

        public DetailSpecifications? DetailSpecifications { get; set; }
    }
}
