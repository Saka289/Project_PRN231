﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Net;
using Web.Services.ProductAPI.Models.Dto;
using Web.Services.ProductAPI.Service;
using Web.Services.ProductAPI.Service.IService;

namespace Web.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductAPIController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetAllProducts();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }


        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetProductById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> AddAsync([FromForm] ProductDtoForCreateAndUpdate productDto)
        {

            var rs = _productService.Add(productDto);

            if (rs != null)
            {
                return Ok(rs);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteById(int id)
        {
            var rs = _productService.Delete(id);
            if (rs != null)
            {
                return Ok(rs);
            }
            return NotFound();
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult Update([FromForm] ProductDtoForCreateAndUpdate productDto)
        {
            var result = _productService.Update(productDto);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }


        [HttpDelete]
        [Route("DeleteSoft/{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteSoftById(int id)
        {
            var rs = _productService.DeleteSoft(id);
            if (rs != null)
            {
                return Ok(rs);
            }
            return NotFound();
        }



    }
}
