using Web.Services.PaymentAPI.Models;

namespace Web.Services.PaymentAPI.Repository.IRepository
{
    public interface IPaymentRepository
    {
        void Remove(int productId);
        void Update(Payments payments);
        Payments FindById(String id);
        List<Payments> FindAll();
    }
}
