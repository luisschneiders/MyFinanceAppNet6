using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace MainApp.Services;

public class EssentialsAPIService : IAPIService
{
    private readonly HttpClient _httpClient;
    private readonly IWebApiService _webApiService;

    public EssentialsAPIService(HttpClient httpClient, IWebApiService webApiService)
    {
        _httpClient = httpClient;
        _webApiService = webApiService;
    }

    public async Task<Response<string>> GetTokenWithBasicAuthAsync(BasicAuthenticationData auth)
    {
        try
        {
            var client = _webApiService.CreateEssentialsHttpClient();

            string json = JsonConvert.SerializeObject(auth);

            StringContent content = new(json, Encoding.UTF8, "application/json");

            var byteArray = Encoding.ASCII.GetBytes($"{auth.Username}:{auth.Password}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.PostAsync(EndPoint.AuthenticationToken, content);

            response.EnsureSuccessStatusCode();

            string token = await response.Content.ReadAsStringAsync();

            Response<string> result = new()
            {
                Data = token,
                Success = true,
            };

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            return new Response<string>()
            {
                Data = string.Empty,
                Success = false,
                ErrorMessage = "Essential API says: " + ex.Message,
            };
        }
    }

    public Task<string> GetBaseUrl()
    {
        string baseUrl = string.Empty;

        if (_httpClient.BaseAddress is not null)
        {
            baseUrl = _httpClient.BaseAddress.ToString();
        }
        
        return Task.FromResult(baseUrl);
    }
}
