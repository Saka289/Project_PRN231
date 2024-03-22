using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web.Services.ProductAPI.Data;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;
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
        public void AddAsync(ProductImageDto p)
        {
            if (p != null)
            {
                ProductImage p1 = new ProductImage();
                p1.Image = p.Image;
                p1.IsDefault = p.IsDefault; 
                p1.ProductId = p.ProductId;
                _context.ProductImages.Add(p1);

            }
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
