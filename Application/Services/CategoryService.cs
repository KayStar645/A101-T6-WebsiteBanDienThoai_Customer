using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Application.Responses;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepo = categoryRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<List<CategoryDto>>> GetAll()
        {
            var query = _categoryRepo.GetAllInclude();

            int totalCount = await query.CountAsync();

            var products = await query.ToListAsync();

            var mapProducts = _mapper.Map<List<CategoryDto>>(products);

            return PaginatedResult<List<CategoryDto>>.Success(
                mapProducts, totalCount, 1,
                totalCount);
        }
    }
}
