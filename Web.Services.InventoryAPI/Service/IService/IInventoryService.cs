using Web.Services.InventoryAPI.Models.Dto;

namespace Web.Services.InventoryAPI.Service.IService
{
    public interface IInventoryService
    {
        public List<InventoryDTO> isInStock(List<ProductRequest> products);
    }
}


