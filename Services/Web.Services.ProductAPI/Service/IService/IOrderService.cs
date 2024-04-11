using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI.Service.IService 
{ 
    public interface IOrderService
    {
        Task<IEnumerable<BestSeller>> GetBestSellers();
    }
}
