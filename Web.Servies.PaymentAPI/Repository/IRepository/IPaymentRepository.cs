using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Models.Dto;

namespace Web.Services.PaymentAPI.Repository.IRepository
{
    public interface IPaymentRepository
    {
        void Remove(int productId);
        Payments Update(Payments payments);
        Payments Create(Payments payments);
        Payments FindById(string id);
        List<Payments> FindAll();
        Payments FindByOrderId(string orderId);
        void Save();
    }
}
