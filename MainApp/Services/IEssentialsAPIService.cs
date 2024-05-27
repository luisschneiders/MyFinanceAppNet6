namespace MainApp.Services;

public interface IEssentialsAPIService
{
    HttpClient CreateHttpClient();
    Task<Response<string>> GetTokenWithBasicAuthAsync();
    Task<string> GetBaseUrl();
    Task<bool> IsTokenExpired();
}
