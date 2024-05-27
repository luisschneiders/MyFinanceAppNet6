using System.Net.Http.Headers;

namespace MainApp.Services;

public class FinnhubService : IFinnhubService
{
    private readonly IEssentialsAPIService _essentialsAPIService;

    public FinnhubService(IEssentialsAPIService essentialsAPIService)
    {
        _essentialsAPIService = essentialsAPIService;
    }

    public async Task<Response<List<FinnhubNewsModel>>> GetNewsAsync()
    {
        try
        {
            var client = _essentialsAPIService.CreateHttpClient();

            // Retrieve token for authorization
            Response<string> authResponse = await _essentialsAPIService.GetTokenWithBasicAuthAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Data);

            Response<List<FinnhubNewsModel>>? response = await client.GetFromJsonAsync<Response<List<FinnhubNewsModel>>>(EndPoint.V2FinnhubNewsAll);

            return await Task.FromResult(response!);

        }
        catch (Exception ex)
        {
            return new Response<List<FinnhubNewsModel>>()
            {
                Data = new List<FinnhubNewsModel>(),
                Success = false,
                ErrorMessage = "Finnhub says: " + ex.Message,
            };
        }
    }
}
