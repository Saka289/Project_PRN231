using Web.Services.InventoryAPI.Models;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Repository.IRepository;
using Web.Services.InventoryAPI.Service.IService;

namespace Web.Services.InventoryAPI.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IIventoryRepository _repository;

        public InventoryService(IIventoryRepository repository)
        {
            _repository = repository;
        }

        public List<InventoryDTO> isInStock(List<ProductRequest> products)
        {
            List<Inventory> inventories = _repository.GetAll();
            List<InventoryDTO> inventoryDTOs = new List<InventoryDTO>();

            foreach(ProductRequest product in products)
            {
                InventoryDTO inventoryDTO = new InventoryDTO();
                Inventory inventory = inventories.SingleOrDefault(x => x.ProductId == product.ProductId);
                if(inventory != null)
                {
                    inventoryDTO.productId = inventory.ProductId;
                    if (inventory.ReservedQuantity > 0)
                        inventoryDTO.isInStock = true;
                    else inventoryDTO.isInStock = false;


                    if(inventory.ReservedQuantity < product.Quantity)
                    {
                        inventoryDTO.isEnoughtQty = false;
                    }
                    else inventoryDTO.isEnoughtQty = true;

                    inventoryDTOs.Add(inventoryDTO);
                }
            }
            return inventoryDTOs;
        }

        public void UpdateInventory(List<ProductRequest> products, string status)
        {
            List<Inventory> inventories = _repository.GetAll();
            if (status.Equals("Order Success"))
            {
                foreach (ProductRequest product in products)
                {
                    Inventory inventory = inventories.SingleOrDefault(x => x.ProductId == product.ProductId);
                    if (inventory != null)
                    {
                        inventory.ReservedQuantity += product.Quantity;
                        inventory.StockQuantity -= product.Quantity;
                    }
                }
            } else if (status.Equals("Roll back"))
            {
                foreach (ProductRequest product in products)
                {
                    Inventory inventory = inventories.SingleOrDefault(x => x.ProductId == product.ProductId);
                    if (inventory != null)
                    {
                        inventory.ReservedQuantity -= product.Quantity;
                        inventory.StockQuantity += product.Quantity;
                    }
                }
            }
        }

    }
}
