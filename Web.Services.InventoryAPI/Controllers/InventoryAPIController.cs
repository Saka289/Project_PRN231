using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System.Data;
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
        private IWebHostEnvironment _webHostEnvironment;

        public InventoryAPIController(IInventoryService inventoryService, IWebHostEnvironment webHostEnvironment)
        {
            _inventoryService = inventoryService;
            responseDto = new ResponseDto();
            _webHostEnvironment = webHostEnvironment;
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

            if (inven == null)
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

            if (update == 0)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = "Update failes";
                return BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.Message = "Update Successfully";
            return Ok(responseDto);
        }

        [HttpPost("ImportCsv")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> ImportCsv([FromForm] IFormFile formFile)
        {
            var item =  await _inventoryService.Upload(formFile);
            if(item > 0)
            {
                responseDto.IsSuccess = true;
                responseDto.Result = item;
                responseDto.Message = "Import success " + item + " Record";
            } else
            {
                responseDto.IsSuccess = false;
                responseDto.Result = item;
                responseDto.Message = "No Item to Import ";
            }
            return Ok(responseDto);
        }
    }
        
}
