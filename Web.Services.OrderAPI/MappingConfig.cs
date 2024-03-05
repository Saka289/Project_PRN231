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
                config.CreateMap<CartOrderDto, OrderDto>().ReverseMap();
                config.CreateMap<CartOrderDetailDto, OrderDetailDto>().ReverseMap();
                config.CreateMap<OrderDto, Order>().ReverseMap();
                config.CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
