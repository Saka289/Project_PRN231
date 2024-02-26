using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using FileUpload.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3Object = FileUpload.Models.S3Object;

namespace FileUpload
{
    public class FileUploadFunction
    {
        public  async Task<string> UploadImageAsync(S3Object s3obj, AwsCredentials awsCredentials)
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
            var publicUrl = "";
            if (s3obj.BucketName == "productimages")
            {
                publicUrl = $"https://pub-191ecd9a1583498b988625f3d5e36e89.r2.dev/{s3obj.Name}";
            }
            else if(s3obj.BucketName == "categoryimages")
            {
                publicUrl = $"https://pub-e70a7d818fa94d5dbed122bb99c1bbb3.r2.dev/{s3obj.Name}";
            }
            return publicUrl;
        }



        public async Task DeleteFileAsync(string bucketName, string key)
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
            await _s3Client.DeleteObjectAsync(bucketName, key);
        }


    }
}
