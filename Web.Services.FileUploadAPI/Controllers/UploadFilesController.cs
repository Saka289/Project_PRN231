using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Web.Services.FileUploadAPI.Models;
using Web.Services.FileUploadAPI.Services;
using static System.Net.WebRequestMethods;
using S3Object = Web.Services.FileUploadAPI.Models.S3Object;

namespace Web.Services.FileUploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        private readonly ILogger<UploadFilesController> _logger;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        public UploadFilesController(ILogger<UploadFilesController> logger, IStorageService storageService, IConfiguration configuration)
        {
            _logger = logger;   
            _storageService = storageService;
            _configuration = configuration;
        }
        [HttpPost(Name = "UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Process the file 
            // convert this file to memorystream to we can pass it to s3obj
            await using var memoryStr = new MemoryStream();
            // copy content of file to memorystream
            await file.CopyToAsync(memoryStr);

            var fileExtension = Path.GetExtension(file.FileName); // .jpg

            var objName = $"{Guid.NewGuid()}-{Path.GetFileNameWithoutExtension(file.FileName)}{fileExtension}";

            var s3Obj = new S3Object
            {
                BucketName = "productimages",
                InputStream = memoryStr,
                Name = objName,
            };
            var cred = new AwsCredentials()
            {
                // get info from appsetting.json 
                AwsKey = _configuration["AwsConfiguration:AwsAccessKey"],
                AwsSecretKey = _configuration["AwsConfiguration:AwsScretKey"],
                
            };
            var rs =   await _storageService.UploadFileAsync(s3Obj,cred);
            var urlForDb = "https://pub-191ecd9a1583498b988625f3d5e36e89.r2.dev/" + objName;


			return Ok(urlForDb);

            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilesAsync(string? prefix)
        {
            var cred = new AwsCredentials()
            {
                // get info from appsetting.json 
                AwsKey = _configuration["AwsConfiguration:AwsAccessKey"],
                AwsSecretKey = _configuration["AwsConfiguration:AwsScretKey"],

            };
            var obj = await _storageService.GetAllFileAsync(prefix, cred);
            return Ok(obj);
        }

        [HttpGet("preview")]
        public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
        {
            var _s3Client = new AmazonS3Client(
              "187a838703ea90ac7984002e8c0f4425",
              "044c4cbdd24628a314a263bff4fb59fcea8ceb64e49d92240c22986796503f07",
              new AmazonS3Config
              {
                  ServiceURL = "https://629519f895d0849984d96055afded11a.r2.cloudflarestorage.com",
                  SignatureVersion = "4", // Sử dụng chữ ký xác thực V4
                  ForcePathStyle = true // Sử dụng dạng đường dẫn cho URL
              });
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
            return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string bucketName, string key)
        {
            var _s3Client = new AmazonS3Client(
              "187a838703ea90ac7984002e8c0f4425",
              "044c4cbdd24628a314a263bff4fb59fcea8ceb64e49d92240c22986796503f07",
              new AmazonS3Config
              {
                  ServiceURL = "https://629519f895d0849984d96055afded11a.r2.cloudflarestorage.com",
                  SignatureVersion = "4", // Sử dụng chữ ký xác thực V4
                  ForcePathStyle = true // Sử dụng dạng đường dẫn cho URL
              });
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist");
            await _s3Client.DeleteObjectAsync(bucketName, key);
            return NoContent();
        }

    }
}
