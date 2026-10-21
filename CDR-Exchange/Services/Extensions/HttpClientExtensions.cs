using System.Net.Http.Headers;
using System.Net.Mime;
using Newtonsoft.Json;

namespace CDR_Exchange.Services.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            return httpClient.PostAsync(url, content);
        }
        
        public static async Task<TResponse> PostAsJsonAndGetContentAsync<TRequest, TResponse>(this HttpClient httpClient, string url, TRequest data)
        {
            var responseMessage = await PostAsJsonAsync(httpClient, url, data);
            var contentAsString = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(contentAsString);
        }
        
        public static async Task<string> PostAsJsonAndGetStringAsync<TRequest>(this HttpClient httpClient, string url, TRequest data)
        {
            var responseMessage = await PostAsJsonAsync(httpClient, url, data);
            return await responseMessage.Content.ReadAsStringAsync();
        }
        
        public static async Task<string> GetAndReturnStringAsync(this HttpClient httpClient, string url)
        {
            var responseMessage = await httpClient.GetAsync(url);
            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}