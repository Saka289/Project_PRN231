using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web.Services.ProductAPI.Data;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Repository.IRepository;

namespace Web.Services.ProductAPI.Repository
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddAsync(ProductImage p)
        {
            _context.ProductImages.Add(p);
        }

        public void ChangeDefaultImageAsync(int id)
        {
            var productImage = _context.ProductImages.Find(id);
            if (productImage != null)
            {
                productImage.IsDefault = true;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<ProductImage>> GetAllByProductIdAsyns(int productId)
        {
            return await _context.ProductImages.Where(x=>x.ProductId==productId).ToListAsync();
        }

        public ProductImage GetByIdAsyns(int productImageId)
        {
            var pI = _context.ProductImages.Find(productImageId);
            if (pI != null)
            {
                return pI;
            }
            else
            {
                return null;
            }
        }

        public void Remove(int productImageId)
        {
            var pI = _context.ProductImages.Find(productImageId);
            if (pI != null)
            {
                _context.ProductImages.Remove(pI);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
