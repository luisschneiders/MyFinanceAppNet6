using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace MainApp.Services;

public class EssentialsAPIService : IAPIService
{
    private readonly HttpClient _httpClient;
    private readonly IWebApiService _webApiService;
    private string _token { get; set; } = string.Empty;

    public EssentialsAPIService(HttpClient httpClient, IWebApiService webApiService)
    {
        _httpClient = httpClient;
        _webApiService = webApiService;
    }

    public async Task<Response<string>> GetTokenWithBasicAuthAsync(BasicAuthenticationData auth)
    {
        try
        {
            Response<string> result = new();

            if (IsTokenExpired())
            {
                result = await GetToken(auth);
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

    public bool IsTokenExpired()
    {
        try
        {
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

    private async Task<Response<string>> GetToken(BasicAuthenticationData auth)
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

            _token = await response.Content.ReadAsStringAsync();

            Response<string> result = new()
            {
                Data = _token,
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
}
