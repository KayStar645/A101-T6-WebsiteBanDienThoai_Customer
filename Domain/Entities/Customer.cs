using Domain.Common;

namespace Domain.Entities
{
    public class Customer : Entity
    {
        public string? InternalCode { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}
