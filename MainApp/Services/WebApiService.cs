using System.Net.Http;

namespace MainApp.Services;

public class WebApiService : IWebApiService
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _httpClient;

    public WebApiService(IHttpClientFactory factory, HttpClient httpClient)
    {
        _factory = factory;
        _httpClient = httpClient;
    }

    public HttpClient CreateEssentialsHttpClient()
    {
        return _factory.CreateClient("essentials-api");
    }
}
