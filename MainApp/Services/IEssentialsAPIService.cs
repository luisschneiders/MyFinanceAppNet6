namespace MainApp.Services;

public interface IEssentialsAPIService
{
    HttpClient CreateHttpClient();
    Task<Response<string>> GetTokenWithBasicAuthAsync(BasicAuthenticationData auth);
    Task<string> GetBaseUrl();
    public bool IsTokenExpired();
}
