using Web.Services.ProductAPI.Models;

namespace Web.Services.ProductAPI.Repository.IRepository
{
    public interface ICategoryRepository: IDisposable
    {
        Task<IEnumerable<Category>> GetAllAsyns();
        Category GetByIdAsyns(int cId);
        void AddAsync(Category c);
        void Remove(int cId);
        void UpdateAsync(Category c);
        void Save();

        void RemoveSoft(int cId);
    }
}
