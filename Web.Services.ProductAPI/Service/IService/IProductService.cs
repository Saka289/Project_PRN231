using Shared.Dtos;
using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProducts();
        ResponseDto GetProductById(int pId);
        Task<ResponseDto> Add(ProductDtoForCreateAndUpdate model);
        Task<ResponseDto> Update(ProductDtoForCreateAndUpdate model);
        ResponseDto Delete(int pId);
        ResponseDto DeleteSoft(int pId);
    }
}
