using AutoMapper;
using Shared.Dtos;
using Web.Services.CouponAPI.Models;
using Web.Services.CouponAPI.Models.Dto;
using Web.Services.CouponAPI.Repository.IRepository;
using Web.Services.CouponAPI.Service.IService;

namespace Web.Services.CouponAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto _response;
        private IMapper _mapper;

        public CouponService(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        public ResponseDto Add(CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _couponRepository.AddAsync(obj);
                _couponRepository.Save();
                _response.Result = _mapper.Map<CouponDto>(obj);
                _response.Message = "Coupon created Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto Delete(int couponId)
        {
            try
            {
                _couponRepository.Remove(couponId);
                _couponRepository.Save();
                _response.Result = true;
                _response.Message = "Coupon deleted Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GetAllCoupons()
        {
            try
            {
                IEnumerable<Coupon> objList = await _couponRepository.GetAllAsyns();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _couponRepository.GetByCode(code);
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto GetCouponById(int couponId)
        {
            try
            {
                Coupon coupon = _couponRepository.GetAsyns(couponId);
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto Update(CouponDto couponDto)
        {
            try
            {
                _couponRepository.UpdateAsync(_mapper.Map<Coupon>(couponDto));
                _couponRepository.Save();
                _response.Result = true;
                _response.Message = "Coupon updated Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
