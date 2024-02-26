using Microsoft.AspNetCore.Mvc;
using Web.Services.FileUploadAPI.Models;

namespace Web.Services.FileUploadAPI.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(S3Object s3obj, AwsCredentials awsCredentials);
        Task<IEnumerable<S3ResponseDto>> GetAllFileAsync(string? prefix, AwsCredentials awsCredentials);

       
    }
}
