using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Services.OrderAPI.Models.Dto
{
    public class OrderDto
    {
        public string orderId { get; set; }
        public DateTime orderDate { get; set; }
        public string orderDesc { get; set; }
        public Double orderFee { get; set; }
        public string shippingAddress { get; set; }
        public List<OrderDetailDTO> orderLineItemsDtoList { get; set; }
        public string userId { get; set; }
    }
}
