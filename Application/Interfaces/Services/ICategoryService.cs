using Core.Application.Responses;
using Domain.DTOs;

namespace Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<PaginatedResult<List<CategoryDto>>> GetAll();
    }
}
