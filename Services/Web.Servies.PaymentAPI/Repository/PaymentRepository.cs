using AutoMapper;
using Web.Services.PaymentAPI.Data;
using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Repository.IRepository;

namespace Web.Services.PaymentAPI.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PaymentRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Remove(int paymentId)
        {
            var obj = _context.Payments.SingleOrDefault(x => x.id.Equals(paymentId));
            if (obj != null)
            {
                _context.Payments.Remove(obj);
            }
        }

        public Payments Update(Payments payments)
        {
            var obj = _context.Payments.FirstOrDefault(p => p.id == payments.id);
            if (obj != null)
            {
                obj.orderId = payments.orderId;
                obj.isPayed = payments.isPayed;
                obj.paymentStatus = payments.paymentStatus;
                obj.refund = payments.refund;
                return obj;
            }
            return null;
        }

        public List<Payments> FindAll()
        {
            return _context.Payments.ToList();
        }

        public Payments FindById(string id)
        {
            return _context.Payments.FirstOrDefault(p => p.id.ToString().Equals(id));
        }

        public Payments FindByOrderId(string orderId)
        {
            var result = _context.Payments.FirstOrDefault(p => p.orderId.Equals(orderId));
            return result;
        }

        public Payments Create(Payments payments)
        {
            return _context.Set<Payments>().Add(payments).Entity;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
