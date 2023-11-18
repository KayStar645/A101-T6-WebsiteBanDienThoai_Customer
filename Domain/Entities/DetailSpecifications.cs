using Domain.Common;

namespace Domain.Entities
{
    public class DetailSpecifications : Entity
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? SpecificationsId { get; set; }

        public Specifications? Specifications { get; set; }
    }
}
