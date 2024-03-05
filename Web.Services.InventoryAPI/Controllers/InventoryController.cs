using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Service.IService;

namespace Web.Services.InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private ResponseDto responseDto;

        public InventoryController(IInventoryService inventoryService) 
        {
            _inventoryService = inventoryService;
            responseDto = new ResponseDto();
        }

        [HttpPost]
        public IActionResult isInStock([FromBody] List<ProductRequest> products)
        {

            var invens = _inventoryService.isInStock(products);
            if(invens == null)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = "Not product is in stock";
                return NotFound(responseDto);
            }
            responseDto.IsSuccess = true;
            responseDto.Message = "have result";
            responseDto.Result = invens;
            

            return Ok(responseDto);
        }
    }
}
