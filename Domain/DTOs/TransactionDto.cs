namespace Domain.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public List<int> ProductIds { get; set; }
    }
}
