namespace Web.Services.FileUploadAPI.Models
{
    public class S3Object
    {
        public string Name { get; set; } = null!;
        // về cơ bản MemoryStream này sẽ là tệp trong mảng byte và mảng byte đó sẽ được tải lên s3
        public MemoryStream InputStream { get; set; } = null!;
        public string BucketName { get; set; } = null!;
    }
}
