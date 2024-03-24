using AutoMapper;
using Web.Services.OrderAPI.Models;
using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderDto, CartHeaderDto>().ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();
                config.CreateMap<CartDetailsDto, OrderDetailDto>().ReverseMap();
                config.CreateMap<OrderDto, Order>().ReverseMap();
                config.CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
