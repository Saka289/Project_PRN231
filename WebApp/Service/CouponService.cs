using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> DeleteCouponById(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/CouponAPI/" + id,
            });
        }

        public async Task<ResponseDto?> GetAllCoupon()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/CouponAPI"
            });
        }

        public async Task<ResponseDto?> GetCouponById(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/CouponAPI/" + id,
            });
        }

        public async Task<ResponseDto?> UpdateCounpon(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = couponDto,
                Url = SD.BaseUrlGateWay + "/api/CouponAPI"
            });
        }
    }
}
