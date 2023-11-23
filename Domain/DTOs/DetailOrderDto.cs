namespace Domain.DTOs
{
    public class DetailOrderDto
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public ProductDto Product { get; set; }

        public long DiscountPrice { get; set; }

        public long Price { get; set; }

        public long SumPrice { get; set; }

        public int Quantity { get; set; }
    }
}
