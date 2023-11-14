using Domain.Entities;

namespace Domain.ViewModels
{
    public class ProductVM
    {
        public string? Name { get; set; }

        public List<Color>? Colors { get; set; }

        public List<Capacity>? Capacitis { get; set; }

        public List<Product>? Products { get; set; }
    }
}
