namespace Web.Services.ShoppingCartAPI.Models
{
    public class CartDTO
    {
        public String userId {get; set;} 
        public ISet<OrderDetailDTO> orderDetailDTOs { get; set;}
    }
}
