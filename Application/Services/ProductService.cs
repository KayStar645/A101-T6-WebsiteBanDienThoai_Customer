using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Application.Responses;
using Domain.DTOs;
using Domain.Entities;
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

        // Chưa áp dụng CTKM
        public async Task<PaginatedResult<List<ProductDto>>> GetAll(ListProductRequest pRequest)
        {
            var query = _productRepo.GetAllInclude();

            query = query.Where(x => x.CategoryId == pRequest.CategoryId && x.IsDeleted == false);

            int totalCount = await query.CountAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            query = query.GroupBy(x => x.Name).Select(group => new Product
            {
                Name = group.Key,
                Id = group.FirstOrDefault().Id,
                InternalCode = group.FirstOrDefault().InternalCode,
                Images = group.FirstOrDefault().Images,
                Quantity = group.FirstOrDefault().Quantity,
                Price = group.FirstOrDefault().Price,
            });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

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
