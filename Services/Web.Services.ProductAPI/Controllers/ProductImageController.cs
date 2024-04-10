using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Net;
using Web.Services.ProductAPI.Models.Dto;
using Web.Services.ProductAPI.Service;
using Web.Services.ProductAPI.Service.IService;

namespace Web.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService; 
        }
        [HttpGet]
        [Route("{productId:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> Get(int productId)
        {
            var result = await _productImageService.GetListImageOfProduct(productId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet]
        [Route("GetById/{id}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productImageService.GetProductImageById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> AddAsync([FromForm] UploadDto model)
        {
            var rs = await _productImageService.UploadMultiImage(model.ProductId, model.files);
             
            if (rs != null)
            {
                return Ok(rs);
            }
            return BadRequest();
        }
        [HttpDelete]
        [Route("{Id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int Id)
        {
            var rs = await  _productImageService.DeleteImageOfProductImageAsync(Id);

            if (rs != null)
            {
                return Ok(rs);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("{id:int}/{pid:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeDefaultImageAsync(int id, int pid)
        {
            var rs = await _productImageService.ChangeDefaultImage(id, pid);
            if (rs != null)
            {
                return Ok(rs);
            }
            return BadRequest();
        }
    }
}
