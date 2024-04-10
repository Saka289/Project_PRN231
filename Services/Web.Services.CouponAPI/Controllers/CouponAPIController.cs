using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Enums;
using System.Net;
using Web.Services.CouponAPI.Models.Dto;
using Web.Services.CouponAPI.Service.IService;

namespace Web.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponAPIController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _couponService.GetAllCoupons();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult Get(int id)
        {
            var result = _couponService.GetCouponById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult Get(string code)
        {
            var result = _couponService.GetByCode(code);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody] CouponDto couponDto)
        {
            var result = _couponService.Add(couponDto);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPut]
        [Authorize(Roles = SD.RoleAdmin)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult Put([FromBody] CouponDto couponDto)
        {
            var result = _couponService.Update(couponDto);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpDelete]
        [Authorize(Roles = SD.RoleAdmin)]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult Delete(int id)
        {
            var result = _couponService.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

    }
}
