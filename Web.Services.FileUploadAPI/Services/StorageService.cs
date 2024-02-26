using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using Web.Services.FileUploadAPI.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using S3Object = Web.Services.FileUploadAPI.Models.S3Object;

namespace Web.Services.FileUploadAPI.Services
{
    public class StorageService : IStorageService
    {
        public async Task<IEnumerable<S3ResponseDto>> GetAllFileAsync(string? prefix, AwsCredentials awsCredentials)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = "productimages",
                Prefix = prefix
            };
            var s3Client = new AmazonS3Client(
              awsCredentials.AwsKey,
              awsCredentials.AwsSecretKey,
              new AmazonS3Config
              {
                  ServiceURL = "https://629519f895d0849984d96055afded11a.r2.cloudflarestorage.com",
                  ForcePathStyle = true, // Sử dụng dạng đường dẫn cho URL
                 
              });

            try
            {
                // Gửi yêu cầu để liệt kê tất cả các đối tượng trong bucket
                var listObjectsResponse = await s3Client.ListObjectsV2Async(request);

                // Lấy danh sách các đối tượng từ kết quả trả về
                var s3Objects = listObjectsResponse.S3Objects;

                // Tạo một danh sách mới chứa thông tin về URL của mỗi đối tượng
                var s3ResponseList = new List<S3ResponseDto>();

                // Duyệt qua từng đối tượng trong danh sách
                foreach (var s3Object in s3Objects)
                {
                    AWSConfigsS3.UseSignatureVersion4 = true;

                    var urlR2DevImage = "https://pub-191ecd9a1583498b988625f3d5e36e89.r2.dev/"+s3Object.Key;

                    // Tạo yêu cầu để lấy URL của đối tượng
                    //var presign = new GetPreSignedUrlRequest
                    //{
                    //    BucketName = "productimages",
                    //    Key = s3Object.Key,
                    //    Expires = DateTime.Now.AddDays(29),
                    //    Protocol = Protocol.HTTPS, // Ensure HTTPS protocol.
                    //    Verb = HttpVerb.GET // Specify HTTP method
                    //};

                    // Lấy URL đã ký hợp đồng cho đối tượng từ S3
                   // var presignedUrl =  s3Client.GetPreSignedURL(presign);
                    // Tạo một đối tượng DTO chứa URL và thêm vào danh sách
                    var s3ResponseDto = new S3ResponseDto
                    {
                        Url = urlR2DevImage,
                        Message = ""
                    };
                    s3ResponseList.Add(s3ResponseDto);
                }
                return s3ResponseList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UploadFileAsync(S3Object s3obj, AwsCredentials awsCredentials)
        {

            AWSConfigsS3.UseSignatureVersion4 = true;

            var s3Client = new AmazonS3Client(
               awsCredentials.AwsKey,
               awsCredentials.AwsSecretKey,
               new AmazonS3Config
               {
                   ServiceURL = "https://629519f895d0849984d96055afded11a.r2.cloudflarestorage.com",
               });


            var respone = new S3ResponseDto();
            //Create the upload request
            var uploadRequest = new PutObjectRequest()
            {
                BucketName = s3obj.BucketName,
                Key = s3obj.Name,
                InputStream = s3obj.InputStream,
                DisablePayloadSigning = true
            };

            var response = await s3Client.PutObjectAsync(uploadRequest);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK && response.HttpStatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception("Upload to Cloudflare R2 failed");
            }
            // Tạo URL public
            var publicUrl = $"https://{s3obj.BucketName}.629519f895d0849984d96055afded11a.r2.app/{s3obj.Name}";
            return publicUrl;
        }



    }


}
