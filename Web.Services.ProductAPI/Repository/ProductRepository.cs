using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web.Services.ProductAPI.Data;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;
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
            return await _appDbContext.Products.Include(x=>x.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCateAsyns(int id)
        {
            return await _appDbContext.Products.Include(x => x.Category).Where(x=>x.CategoryId==id).ToListAsync();
        }

        public Product GetByIdAsyns(int pId)
        {
            var p = _appDbContext.Products.Include(x=>x.Category).FirstOrDefault(x=>x.Id==pId);
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

        public async Task<IEnumerable<Product>> SearchAsyns(string searchValue)
        {
            return await _appDbContext.Products.Include(x => x.Category)
                .Where(x=>x.Title.ToLower().Contains(searchValue.ToLower()) || x.ProductCode.ToLower().Contains(searchValue.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchInShopPageAsyns(ProductSearchDto searchModel)
        {
            var query = _appDbContext.Products.Include(x=>x.Category).Where(x=>x.Status== "Active").AsQueryable();
            if(searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.Title))
                {
                    query = query.Where(x => x.Title.ToLower().Contains(searchModel.Title));
                }
                if (searchModel.CategoryId >0)
                {
                    query = query.Where(x => x.CategoryId==searchModel.CategoryId);
                }
                if (searchModel.PriceFrom >= 0 && searchModel.PriceTo >0 && (searchModel.PriceFrom < searchModel.PriceTo))
                {
                    query = query.Where(x => x.Price>= searchModel.PriceFrom && x.Price<= searchModel.PriceTo);
                }

                if (!string.IsNullOrEmpty(searchModel.sortQuery))
                {
                    if(searchModel.sortQuery == "Price")
                    {
                        query = query.OrderBy(x => x.Price);
                    }
                    if (searchModel.sortQuery == "Title")
                    {
                        query = query.OrderBy(x => x.Title);
                    }
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }
            return query.ToList();      
        }

        public void UpdateAsync(Product p)
        {
            _appDbContext.Products.Update(p);
        }

       
    }
}
