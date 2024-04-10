using System.Data;
using Web.Services.InventoryAPI.Models;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Repository.IRepository;
using Web.Services.InventoryAPI.Service.IService;
using Web.Services.OrderAPI.Service.IService;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using FileUpload;


namespace Web.Services.InventoryAPI.Service
{
    public class InventoryService : IInventoryService
    {
        public readonly IConfiguration _configuration;
        private readonly IIventoryRepository _repository;
        public readonly IProductService _productService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public InventoryService(IIventoryRepository repository, IProductService productService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _repository = repository;
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
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

            foreach (ProductRequest product in products)
            {
                InventoryDTO inventoryDTO = new InventoryDTO();
                Inventory inventory = inventories.SingleOrDefault(x => x.ProductId == product.ProductId);
                if (inventory != null)
                {
                    inventoryDTO.productId = inventory.ProductId;
                    if (inventory.StockQuantity > 0)
                        inventoryDTO.isInStock = true;
                    else inventoryDTO.isInStock = false;


                    if (inventory.StockQuantity < product.Quantity)
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
            }
            else if (status.Equals("Roll back"))
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
            _repository.Save();
        }

        public StockDto GetInventoryById(int id)
        {
            var inven = _repository.GetInventoryById(id);

            if (inven != null)
            {
                return new StockDto
                {
                    Id = id,
                    ProductId = inven.ProductId,
                    StockQuantity = inven.StockQuantity,
                    ReservedQuantity = inven.ReservedQuantity,
                    Product = _productService.GetProductById(inven.ProductId).Result,
                };
            }
            return null;
        }

        public int Update(StockCreate stock)
        {
            var inven = _repository.GetInventoryById(stock.Id);
            if (inven != null)
            {
                if (inven.StockQuantity > 0)
                {
                    inven.StockQuantity = stock.StockQuantity;
                    return _repository.Update(inven);
                }
            }
            return 0;
        }


        public async Task<int> Upload(IFormFile file)
        {
            try
            {
                var filePath = SaveFile(file);

                // load product requests from excel file
                var inventoriedto = ExcelHelper.Import<InventoryImport>(filePath);

                List<Inventory> inventories = inventoriedto.Where(item => item.ProductId != 0).Select(item => new Inventory
                {
                    ProductId = item.ProductId,
                    StockQuantity = item.StockQuantity,
                }).ToList();

                return await _repository.Import(inventories);
            }
            catch
            {
                throw new Exception("Data Invalid type");
            }
        }

        private string SaveFile(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new BadHttpRequestException("File is empty.");
            }

            var extension = Path.GetExtension(file.FileName);

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            }

            var folderPath = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}.{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return filePath;
        }
    }

}
