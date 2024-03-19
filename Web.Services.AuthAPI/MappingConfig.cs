using AutoMapper;
using Web.Services.AuthAPI.Models;
using Web.Services.AuthAPI.Models.Dto;

namespace Web.Services.AuthAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, MemberDto>().ReverseMap();
                config.CreateMap<ApplicationUser, MemberAddEditDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
