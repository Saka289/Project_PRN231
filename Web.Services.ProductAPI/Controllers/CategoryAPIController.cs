using FileUpload;
using FileUpload.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Net;
using Web.Services.ProductAPI.Models.Dto;
using Web.Services.ProductAPI.Service.IService;

namespace Web.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryAPIController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResponseDto>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.GetAllCategories();
            if(result != null)
            {
                return Ok(result);  
            } 
            return NotFound();
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult GetById (int id)
        {
            var result = _categoryService.GetCategoryById(id);
            if(result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> AddAsync([FromForm] CategoryDtoForCreateAndUpdate categoryDto)
        {

            var rs = _categoryService.Add(categoryDto);


            if (rs != null)
            {
                return Ok(rs);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteById(int id) {
            var rs = _categoryService.Delete(id);
            if(rs != null)
            {
                return Ok(rs);
            }
            return NotFound();
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult Update([FromForm] CategoryDtoForCreateAndUpdate categoryDto)
        {
            var result = _categoryService.Update(categoryDto);

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
            var rs = _categoryService.DeleteSoft(id);
            if (rs != null)
            {
                return Ok(rs);
            }
            return NotFound();
        }

        

    }
}
