using Domain.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, List<string>>().ConvertUsing<StringToListTypeConverter>();
            CreateMap<List<string>, string>().ConvertUsing<ListToStringTypeConverter>();
                        
            //CreateMap<Product, ProductDto>()
            //    .ForMember(dest => dest.Promotion, opt => opt.MapFrom(src => src.PromotionProducts.FirstOrDefault().Promotion));

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Promotion, opt => opt.MapFrom(src => src.PromotionProducts.FirstOrDefault().Promotion))
                .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src =>
                    src.ProductParameters
                        .Select(pp => pp.DetailSpecifications)
                        .GroupBy(ds => ds.SpecificationsId)
                        .Select(group => new SpecificationsDto
                        {
                            Id = group.Key.Value,
                            Name = group.First().Specifications.Name,
                            Details = group.Select(ds => new DetailSpecificationsDto
                            {
                                Id = ds.Id,
                                Name = ds.Name,
                                Description = ds.Description
                            }).ToList()
                        })
                        .ToList()));




            CreateMap<ProductDto, Product>();

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Color, ColorDto>().ReverseMap();

            CreateMap<Capacity, CapacityDto>().ReverseMap();

            CreateMap<PromotionProduct, PromotionProductDto>().ReverseMap();

            CreateMap<Promotion, PromotionDto>().ReverseMap();

            CreateMap<Specifications, SpecificationsDto>().ReverseMap(); // Cần sửa đổi nè

            CreateMap<DetailSpecifications, DetailSpecificationsDto>().ReverseMap();
        }

        private class StringToListTypeConverter : ITypeConverter<string, List<string>>
        {
            public List<string> Convert(string source, List<string> destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                {
                    return new List<string>();
                }

                return source.Split(',').ToList();
            }
        }

        private class ListToStringTypeConverter : ITypeConverter<List<string>, string>
        {
            public string Convert(List<string> source, string destination, ResolutionContext context)
            {
                if (source == null || source.Count == 0)
                {
                    return null;
                }

                return string.Join(",", source);
            }
        }
    }
}
