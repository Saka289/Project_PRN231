using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Repository.IRepository
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<BestSeller>> GetOrderDetailRepositoryAsync();
    }
}
