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

            

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Promotion, opt => opt.MapFrom(src => src.PromotionProducts.FirstOrDefault().Promotion));

            CreateMap<ProductDto, Product>();

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Color, ColorDto>().ReverseMap();

            CreateMap<Capacity, CapacityDto>().ReverseMap();

            CreateMap<PromotionProduct, PromotionProductDto>().ReverseMap();

            CreateMap<Promotion, PromotionDto>().ReverseMap();
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
