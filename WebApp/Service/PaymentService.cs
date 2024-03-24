using Shared.Dtos;
using Shared.Enums;
using System.Reflection;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBaseService _baseService;

        public PaymentService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> GetPaymentById(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/Payment/" + id,
            });
        }

        public async Task<ResponseDto?> UpsertPayment(PaymentDto paymentDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = paymentDto,
                Url = SD.BaseUrlGateWay + "/api/Payment"
            });
        }

        public async Task<ResponseDto?> GetPayments()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/Payment"
            });
        }
    }
}
