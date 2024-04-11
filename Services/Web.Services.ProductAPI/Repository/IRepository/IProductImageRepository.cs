using System;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI.Repository.IRepository
{
    public interface IProductImageRepository: IDisposable
    {
        Task<IEnumerable<ProductImage>> GetAllByProductIdAsyns(int idProduct);
        ProductImage GetByIdAsyns(int id);
        void AddAsync(ProductImageDto p);
        void Remove(int pId);
        void ChangeDefaultImageAsync(int id);
        void Save();
    }
}
