﻿namespace Domain.Entities
{
    public class PromotionProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int PromotionId { get; set; }

        public Product Product { get; set; }
        public Promotion Promotion { get; set; }
    }
}
