namespace Domain.DTOs
{
    public class TransactionDto
    {
        public int OrderId { get; set; }

        public List<int> ProductIds { get; set; }
    }
}
