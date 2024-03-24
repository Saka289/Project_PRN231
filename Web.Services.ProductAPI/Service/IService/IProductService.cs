using Shared.Dtos;
using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProducts();
        Task<ResponseDto> GetAllProductByCateAsync(int id);
        Task<ResponseDto> SearchProducts(string seachValue);
        Task<ResponseDto> SearchProductInShopPage(ProductSearchDto model);
        ResponseDto GetProductById(int pId);
        Task<ResponseDto> Add(ProductDtoForCreateAndUpdate model);
        Task<ResponseDto> Update(ProductDtoForCreateAndUpdate model);
        ResponseDto Delete(int pId);
        ResponseDto DeleteSoft(int pId);
    }
}
