namespace MainApp.Services;

public interface IEssentialsAPIService
{
    public HttpClient CreateHttpClient();
    public Task<Response<string>> GetTokenWithBasicAuthAsync();
    public Task<string> GetBaseUrl();
    public Task<bool> IsTokenExpired();
}
