using Web.Services.CouponAPI.Models.Dto;

namespace Web.Services.CouponAPI.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetAllCoupons();
        ResponseDto GetCouponById(int couponId);
        ResponseDto GetByCode(string code);
        ResponseDto Add(CouponDto couponDto);
        ResponseDto Update(CouponDto couponDto);
        ResponseDto Delete(int couponId);
        
    }
}
