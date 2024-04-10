namespace Web.Services.OrderAPI.Models.Dto
{
    public class UpdateInvensRequestDto
    {
        public List<ProductRequest> products { get; set; }
        public string status { get; set; }
    }
}
