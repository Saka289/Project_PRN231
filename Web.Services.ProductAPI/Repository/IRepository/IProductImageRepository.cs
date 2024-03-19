using System;
using Web.Services.ProductAPI.Models;

namespace Web.Services.ProductAPI.Repository.IRepository
{
    public interface IProductImageRepository: IDisposable
    {
        Task<IEnumerable<ProductImage>> GetAllByProductIdAsyns(int idProduct);
        ProductImage GetByIdAsyns(int id);
        void AddAsync(ProductImage p);
        void Remove(int pId);
        void ChangeDefaultImageAsync(int id);
        void Save();
    }
}
