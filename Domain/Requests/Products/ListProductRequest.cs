using Domain.Requests.Common;

namespace Domain.Requests.Products
{
    public class ListProductRequest : ListBaseRequest
    {
        public int CategoryId { get; set; }
    }
}
