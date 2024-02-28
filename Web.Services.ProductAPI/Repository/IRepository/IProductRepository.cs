using Web.Services.ProductAPI.Models;

namespace Web.Services.ProductAPI.Repository.IRepository
{
    public interface IProductRepository : IDisposable
    {
        Task<IEnumerable<Product>> GetAllAsyns();
        Product GetByIdAsyns(int pId);
        void AddAsync(Product p);
        void Remove(int pId);
        void UpdateAsync(Product p);
        void Save();
        void RemoveSoft(int pId);
    }
}
