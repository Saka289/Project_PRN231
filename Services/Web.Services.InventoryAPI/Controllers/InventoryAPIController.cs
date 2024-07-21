using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Enums;
using System.Data;
using Web.Services.InventoryAPI.Data;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Service.IService;

namespace Web.Services.InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InventoryAPIController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private ResponseDto responseDto;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public InventoryAPIController(IInventoryService inventoryService, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _inventoryService = inventoryService;
            responseDto = new ResponseDto();
            _webHostEnvironment = webHostEnvironment;
            _context = context;
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

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Inventories.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = "Not found !!!";
                responseDto.Result = false;
            }
            _context.Inventories.Remove(result);
            await _context.SaveChangesAsync();

            responseDto.IsSuccess = true;
            responseDto.Result = true;
            responseDto.Message = "Delete Successfully";
            return Ok(responseDto);

        }

        [HttpPost("ImportCsv")]
        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<ActionResult> ImportCsv([FromForm] ImportInvens importInvens)
        {
            var item = await _inventoryService.Upload(importInvens.File);
            if (item > 0)
            {
                responseDto.IsSuccess = true;
                responseDto.Result = item;
                responseDto.Message = "Import success " + item + " Record";
            }
            else
            {
                responseDto.IsSuccess = false;
                responseDto.Result = item;
                responseDto.Message = "No Item to Import ";
            }
            return Ok(responseDto);
        }

        [HttpPut("UpdateInventory")]
        public IActionResult UpdateInven(UpdateInvensRequest request)
        {
            _inventoryService.UpdateInventory(request.products, request.status);
            return Ok(responseDto);
        }

    }

}
