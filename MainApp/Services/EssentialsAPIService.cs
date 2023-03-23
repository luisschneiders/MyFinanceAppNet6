namespace MainApp.Services;

public class EssentialsAPIService : IAPIService
{
    private readonly HttpClient _httpClient;

    public EssentialsAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

#nullable disable
    public Task<string> GetBaseUrl()
    {
        return Task.FromResult(_httpClient.BaseAddress.ToString());
    }
#nullable enable
}
