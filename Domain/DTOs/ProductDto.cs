namespace Domain.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public List<string>? Images { get; set; }

        public int? Quantity { get; set; }

        public long? Price { get; set; }

        public long? NewPrice { get; set; }

        public ColorDto? Color { get; set; }

        public CapacityDto? Capacity { get; set; }

        public PromotionDto? Promotion { get; set; }

        public List<SpecificationsDto> Specifications { get; set; }
    }
}
