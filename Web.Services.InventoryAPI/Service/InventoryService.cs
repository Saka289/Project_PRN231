using Web.Services.InventoryAPI.Models;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Repository.IRepository;
using Web.Services.InventoryAPI.Service.IService;
using Web.Services.OrderAPI.Service.IService;

namespace Web.Services.InventoryAPI.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IIventoryRepository _repository;
        public readonly IProductService _productService;

        public InventoryService(IIventoryRepository repository, IProductService productService)
        {
            _repository = repository;
            _productService = productService;
        }

        public List<StockDto> getStock()
        {
            List<ProductDto> products = _productService.GetProduct().Result;

            var inventories = _repository.GetAll().Select(item => new StockDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ReservedQuantity = item.ReservedQuantity,
                StockQuantity = item.StockQuantity,
                Product = products.FirstOrDefault(i => i.Id == item.ProductId)
            }).ToList();

            return inventories;
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
                    if (inventory.StockQuantity > 0)
                        inventoryDTO.isInStock = true;
                    else inventoryDTO.isInStock = false;


                    if(inventory.StockQuantity < product.Quantity)
                    {
                        inventoryDTO.isInStock = false;
                    }
                    else inventoryDTO.isInStock = true;

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
