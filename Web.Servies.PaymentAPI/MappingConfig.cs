using AutoMapper;
using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Models.Dto;

namespace Web.Services.PaymentAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<PaymentDto, Payments>().ReverseMap();
                config.CreateMap<Payments, PaymentDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
