namespace WebApp.Models.Dtos
{
    public class UploadDto
    {
        public int ProductId { get; set; }

        public IFormFile[] files { get; set; }
    }
}
