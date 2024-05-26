namespace MainApp.Services;

public interface IAPIService
{
    Task<Response<string>> GetTokenWithBasicAuthAsync(BasicAuthenticationData auth);
    Task<string> GetBaseUrl();
}
