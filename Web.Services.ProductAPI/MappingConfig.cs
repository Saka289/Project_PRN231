using AutoMapper;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CategoryDto, Category>().ReverseMap();
                config.CreateMap<Product, ProductDto>().ForMember(x=>x.CategoryName, y=>y.MapFrom(src=>src.Category.Name
                )).ReverseMap();
                config.CreateMap<ProductImageDto, ProductImage>().ReverseMap();    
            });
            return mappingConfig;
        }
    }
}
