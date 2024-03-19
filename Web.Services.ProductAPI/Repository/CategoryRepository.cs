using Microsoft.EntityFrameworkCore;
using Web.Services.ProductAPI.Data;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Repository.IRepository;

namespace Web.Services.ProductAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddAsync(Category c)
        {
            _context.Categories.Add(c);
        }

        public void Dispose()
        {
           _context.Dispose();
        }

        public async Task<IEnumerable<Category>> GetAllAsyns()
        {
            return await  _context.Categories.ToListAsync();
        }

        public Category GetByIdAsyns(int cId)
        {
            var c = _context.Categories.Find(cId);
            if(c != null)
            {
                return c;
            }
            else
            {
                return null;
            }
        }

        public void Remove(int cId)
        {
            var c = _context.Categories.Find(cId);
            if(c!= null)
            {
                _context.Categories.Remove(c);
            }
        }

        public void RemoveSoft(int cId)
        {
            var c = _context.Categories.Find(cId);
            if (c != null)
            {
                if(c.Status == StatusConstant.InActive)
                {
                    c.Status = StatusConstant.Active;
                }
                else if(c.Status == StatusConstant.Active)
                {
                    c.Status = StatusConstant.InActive;
                }
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateAsync(Category c)
        {
            _context.Categories.Update(c);
        }
    }
}
