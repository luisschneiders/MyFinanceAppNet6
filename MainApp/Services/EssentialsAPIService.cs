using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace MainApp.Services;

public class EssentialsAPIService : IEssentialsAPIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _factory;
    private ISessionStorageService _sessionStorageService;
    private string _token { get; set; } = string.Empty;

    private BasicAuthenticationData _auth { get; set; } = new();

    public EssentialsAPIService(
        IConfiguration config,
        IHttpClientFactory factory,
        HttpClient httpClient,
        ISessionStorageService sessionStorageService)
    {
        _config = config;
        _factory = factory;
        _httpClient = httpClient;
        _sessionStorageService = sessionStorageService;
        _auth.Username = _config.GetValue<string>("EssentialsApi:Username");
        _auth.Password = _config.GetValue<string>("EssentialsApi:Password");
    }

    public HttpClient CreateHttpClient()
    {
        return _factory.CreateClient(ClientAPI.Essentials);
    }

    public async Task<Response<string>> GetTokenWithBasicAuthAsync()
    {
        try
        {
            Response<string> result = new();

            bool isTokenExpired = await IsTokenExpired();

            if (isTokenExpired is true)
            {
                await _sessionStorageService.RemoveAsync(SessionStorage.EssentialsApiToken);
                result = await FetchTokenAsync();
            }
            else{
                result = new()
                {
                    Data = _token,
                    Success = true,
                };
            }

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

    public async Task<bool> IsTokenExpired()
    {
        try
        {
            _token = await _sessionStorageService.GetAsync<string>(SessionStorage.EssentialsApiToken);

            if (string.IsNullOrWhiteSpace(_token))
            {
                return true;
            }

            var jwtHandler = new JwtSecurityTokenHandler();

            if (jwtHandler.CanReadToken(_token) is false)
            {
                throw new ArgumentException("The token doesn't seem to be in a proper JWT format.");
            }

            var jwtToken = jwtHandler.ReadToken(_token);
            var expirationTime = jwtToken.ValidTo;

            bool isExpired = expirationTime < DateTime.UtcNow;

            return isExpired;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<Response<string>> FetchTokenAsync()
    {
        try
        {
            var client = CreateHttpClient();

            string json = JsonConvert.SerializeObject(_auth);

            StringContent content = new(json, Encoding.UTF8, "application/json");

            var byteArray = Encoding.ASCII.GetBytes($"{_auth.Username}:{_auth.Password}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.PostAsync(EndPoint.AuthenticationToken, content);

            response.EnsureSuccessStatusCode();

            _token = await response.Content.ReadAsStringAsync();

            Response<string> result = new()
            {
                Data = _token,
                Success = true,
            };

            await _sessionStorageService.SetAsync<string>(SessionStorage.EssentialsApiToken, _token);

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
}
