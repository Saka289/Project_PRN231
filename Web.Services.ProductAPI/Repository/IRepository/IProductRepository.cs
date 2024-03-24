using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI.Repository.IRepository
{
    public interface IProductRepository : IDisposable
    {
        Task<IEnumerable<Product>> GetAllAsyns();
        Task<IEnumerable<Product>> GetAllByCateAsyns(int id);
        Product GetByIdAsyns(int pId);
        void AddAsync(Product p);
        void Remove(int pId);
        void UpdateAsync(Product p);
        void Save();
        void RemoveSoft(int pId);

        Task<IEnumerable<Product>> SearchAsyns(string searchValue);
        Task<IEnumerable<Product>> SearchInShopPageAsyns(ProductSearchDto searchModel);

    }
}
