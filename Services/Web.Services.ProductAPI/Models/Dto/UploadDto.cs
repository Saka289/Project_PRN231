namespace Web.Services.ProductAPI.Models.Dto
{
    public class UploadDto
    {
        public int ProductId { get; set; }

        public IFormFile[] files { get; set; }
    }
}
