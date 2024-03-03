using Web.Services.PaymentAPI.Data;
using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Repository.IRepository;

namespace Web.Services.PaymentAPI.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async void Remove(int paymentId)
        {
            var obj = _context.Payments.SingleOrDefault(x => x.id.Equals(paymentId));
            if (obj != null)
            {
                _context.Payments.Remove(obj);
            }
        }

        public void Update(Payments payments)
        {
            _context.Payments.Update(payments);
            _context.SaveChanges();
        }

        public List<Payments> FindAll()
        {
            return _context.Payments.ToList();
        }

        public Payments FindById(string id)
        {
            return _context.Payments.SingleOrDefault(p => p.id.Equals(id));
        }

        public Payments FindByOrderId(string orderId)
        {
            return _context.Payments.SingleOrDefault(p => p.orderId.ToString().Equals(orderId));
        }

    }
}
