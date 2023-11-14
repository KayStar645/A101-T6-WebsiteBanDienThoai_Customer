using Domain.Common;

namespace Domain.Entities
{
    public class Color : Entity
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }
    }
}
