using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Service.IService;

namespace Web.Services.InventoryAPI.Controllers
{
    [Route("api/inventories")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService) 
        {
            _inventoryService = inventoryService;
        }

        [HttpPost]
        public List<InventoryDTO> isInStock([FromBody] List<ProductRequest> products)
        {
            return _inventoryService.isInStock(products);
        }
    }
}
