using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.Services.CouponAPI.Data;
using Web.Services.CouponAPI.Models;
using Web.Services.CouponAPI.Repository.IRepository;

namespace Web.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async void AddAsync(Coupon coupon)
        {
            await _context.Set<Coupon>().AddAsync(coupon);
        }

        public async Task<IEnumerable<Coupon>> GetAllAsyns()
        {
            return await _context.Set<Coupon>().ToListAsync();
        }

        public void Remove(int couponId)
        {
            var obj = _context.Set<Coupon>().First(c => c.CouponId == couponId);
            if (obj != null)
            {
                _context.Coupons.Remove(obj);
            }
        }

        public void UpdateAsync(Coupon coupon)
        {
            var obj = _context.Set<Coupon>().First(c => c.CouponId == coupon.CouponId);
            if (obj != null)
            {
                _mapper.Map(coupon, obj);
            }
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
            return _context.Set<Coupon>().First(c => c.CouponId == couponId);
        }

        public Coupon GetByCode(string code)
        {
            return _context.Set<Coupon>().First(c => c.CouponCode.ToLower() == code.ToLower());
        }
    }
}
