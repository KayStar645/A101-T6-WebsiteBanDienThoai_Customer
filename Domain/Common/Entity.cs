using Domain.Common.Interfaces;

namespace Domain.Common
{
    public class Entity : IEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
