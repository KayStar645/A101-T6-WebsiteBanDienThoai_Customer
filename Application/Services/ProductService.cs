using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Application.Responses;
using Domain.DTOs;
using Domain.Requests.Products;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepo = productRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<List<ProductDto>>> GetAll(ListProductRequest pRequest)
        {
            var query = _productRepo.GetAllInclude();

            // Áp dụng điều kiện lọc
            query = query.Where(x => x.CategoryId == pRequest.CategoryId);

            int totalCount = await query.CountAsync();

            // Áp dụng phân trang
            query = query.Skip((int)((pRequest.Page - 1) * pRequest.PageSize))
                         .Take((int)pRequest.PageSize);

            var products = await query.ToListAsync();

            var mapProducts = _mapper.Map<List<ProductDto>>(products);

            return PaginatedResult<List<ProductDto>>.Success(
                mapProducts, totalCount, pRequest.Page,
                pRequest.PageSize);
        }

    }
}
