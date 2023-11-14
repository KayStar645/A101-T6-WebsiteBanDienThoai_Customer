using Core.Application.Responses;
using Domain.DTOs;
using Domain.Requests.Products;

namespace Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<PaginatedResult<List<ProductDto>>> GetAll(ListProductRequest pRequest);
    }
}
