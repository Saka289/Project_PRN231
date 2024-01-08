using Microsoft.EntityFrameworkCore;
using Web.Services.CouponAPI.Data;
using Web.Services.CouponAPI.Models;
using Web.Services.CouponAPI.Repository.IRepository;

namespace Web.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async void AddAsync(Coupon coupon)
        {
            await _context.AddAsync(coupon);
        }

        public async Task<IEnumerable<Coupon>> GetAllAsyns()
        {
            return await _context.Coupons.ToListAsync();
        }

        public void Remove(int couponId)
        {
            var obj = _context.Coupons.First(c => c.CouponId == couponId);
            if (obj != null)
            {
                _context.Coupons.Remove(obj);
            }
        }

        public void UpdateAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Coupon GetAsyns(int couponId)
        {
            return _context.Coupons.First(c => c.CouponId == couponId);
        }

        public Coupon GetByCode(string code)
        {
            return _context.Coupons.First(c => c.CouponCode.ToLower() == code.ToLower());
        }
    }
}
