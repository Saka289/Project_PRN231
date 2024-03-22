using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Service.IService;

namespace Web.Services.InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryAPIController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private ResponseDto responseDto;

        public InventoryAPIController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            responseDto = new ResponseDto();
        }

        [HttpPost]
        public IActionResult isInStock([FromBody] List<ProductRequest> products)
        {

            var invens = _inventoryService.isInStock(products);
            if (invens == null)
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

        [HttpGet]
        public IActionResult StockManager()
        {
            var invens = _inventoryService.getStock();

            if (invens == null)
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

        [HttpGet("{id}")]
        public IActionResult GetStock(int id)
        {
            var inven = _inventoryService.GetInventoryById(id);

            if(inven == null)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = "Not stock is existed";
                return NotFound(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.Message = "have result";
            responseDto.Result = inven;

            return Ok(responseDto);
        }

        [HttpPut]
        public IActionResult UpdateStock(StockCreate stock)
        {
            int update = _inventoryService.Update(stock);

            if(update == 0)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = "Update failes";
                return BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.Message = "Update Successfully";
            return Ok(responseDto);
        }
    }
}
