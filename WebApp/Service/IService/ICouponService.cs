using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface ICouponService
    {
        public Task<ResponseDto?> GetAllCoupon();
        public Task<ResponseDto?> GetCouponById(int id);
        public Task<ResponseDto?> UpdateCounpon(CouponDto dto);
        public Task<ResponseDto?> DeleteCouponById(int id);
    }
}
