using Shared.Dtos;
using static Shared.Enums.SD;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CallService
{
    public class SendService : ISendService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SendService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto?> SendServiceAsync(SendRequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("CallAPI");
                HttpRequestMessage message = new();
                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                }

                message.RequestUri = new Uri(requestDto.Url.ToString());

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var responseApi = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        if (responseApi.Result != null && responseApi.IsSuccess)
                        {
                            return responseApi;
                        }
                        else
                        {
                            var apiResponseDto = new ResponseDto
                            {
                                Result = apiContent,
                                IsSuccess = apiResponse.IsSuccessStatusCode,
                                Message = "true"
                            };
                            return apiResponseDto;
                        }
                }
            }
            catch (Exception ex)
            {

                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false,
                };
                return dto;
            }
        }
    }
}

