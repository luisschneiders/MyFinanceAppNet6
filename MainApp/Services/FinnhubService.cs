using MainApp.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace MainApp.Service;

public class FinnhubService : IFinnhubService
{
    private readonly IWebApiService _webApiService;

    public FinnhubService(IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }

    public async Task<Response<List<FinnhubNewsModel>>> GetNewsAsync()
    {
        try
        {
            var client = _webApiService.CreateEssentialsHttpClient();

            Response<List<FinnhubNewsModel>>? response = await client.GetFromJsonAsync<Response<List<FinnhubNewsModel>>>(EndPoint.V2FinnhubNewsAll);

            return await Task.FromResult(response!);

        }
        catch (Exception ex)
        {
            return new Response<List<FinnhubNewsModel>>()
            {
                Data = new List<FinnhubNewsModel>(),
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }
}
