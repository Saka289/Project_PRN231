namespace Web.Services.InventoryAPI.Models.Dto
{
    public class UpdateInvensRequest
    {
        public List<ProductRequest> products { get; set; }
        public string status { get; set; }
    }
}
