using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web.Services.ProductAPI.Data;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Repository.IRepository;

namespace Web.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void AddAsync(Product p)
        {
            _appDbContext.Products.Add(p);
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }

        public async Task<IEnumerable<Product>> GetAllAsyns()
        {
            return await _appDbContext.Products.ToListAsync();
        }

        public Product GetByIdAsyns(int pId)
        {
            var p = _appDbContext.Products.Find(pId);
            if (p != null)
            {
                return p;
            }
            return null;
        }

        public void Remove(int pId)
        {
            var p = _appDbContext.Products.Find(pId);
            if (p != null)
            {
                _appDbContext.Products.Remove(p);
            }
        }

        public void RemoveSoft(int pId)
        {
            var c = _appDbContext.Products.Find(pId);
            if (c != null)
            {
                if (c.Status == StatusConstant.InActive)
                {
                    c.Status = StatusConstant.Active;
                }
                else
                {
                    c.Status = StatusConstant.InActive;
                }
            }
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void UpdateAsync(Product p)
        {
            _appDbContext.Products.Update(p);
        }
    }
}
