using E_Commerce.Models.DTOs;
using E_Commerce.Service.IService;
using Newtonsoft.Json;
using System.Text;
using static E_Commerce.Utility.SD;

namespace E_Commerce.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("E-CommerceAPI");
            HttpRequestMessage msg = new();
            msg.Headers.Add("Accept", "application/json");
            //token

            msg.RequestUri = new Uri(requestDto.Url);
            if(requestDto != null)
            {
                msg.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage? apiResponse = null;

            switch (requestDto.ApiType)
            {
                case ApiType.POST:
                    msg.Method = HttpMethod.Post;
                    break;
                case ApiType.DELETE:
                    msg.Method = HttpMethod.Delete;
                    break;
                case ApiType.PUT:
                    msg.Method = HttpMethod.Put;
                    break;
                default:
                    msg.Method = HttpMethod.Get;
                    break;                      
            }

            apiResponse = await client.SendAsync(msg);

            switch (apiResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new() { Success = false, Message = "Not Found" };
                case System.Net.HttpStatusCode.Forbidden:
                    return new() { Success = false, Message = "Access Denied" };
                case System.Net.HttpStatusCode.Unauthorized:
                    return new() { Success = false, Message = "Unauthorized" };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new() { Success = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
    }
}
