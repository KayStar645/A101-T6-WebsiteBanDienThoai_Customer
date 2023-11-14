namespace Domain.Requests.Common
{
    public class ListBaseRequest
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}
