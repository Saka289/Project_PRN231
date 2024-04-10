using Web.Services.CouponAPI.Models;
using Web.Services.CouponAPI.Models.Dto;

namespace Web.Services.CouponAPI.Repository.IRepository
{
    public interface ICouponRepository : IDisposable
    {
        Task<IEnumerable<Coupon>> GetAllAsyns();
        Coupon GetAsyns(int couponId);
        Coupon GetByCode(string code);
        void AddAsync(Coupon coupon);
        void Remove(int couponId);
        void UpdateAsync(Coupon coupon);
        void Save();
    }
}
