namespace Domain.DTOs
{
    public class PromotionProductDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int PromotionId { get; set; }
        public PromotionDto Promotion { get; set; }
    }
}
