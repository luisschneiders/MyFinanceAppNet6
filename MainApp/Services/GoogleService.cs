using Microsoft.AspNetCore.WebUtilities;

namespace MainApp.Services;

public class GoogleService : IGoogleService
{
    private IWebApiService _webApiService;

    public GoogleService(IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }

    public async Task<Response<List<LocationModel>>> GetGeocodingAddressAsync(string address)
    {
        try
        {
            var client = _webApiService.CreateEssentialsHttpClient();

            var query = new Dictionary<string, string>()
            {
                ["Address"] = address
            };

            var uri = QueryHelpers.AddQueryString(EndPoint.V2GoogleGeocoding, query!);

            Response<List<LocationModel>>? response = await client.GetFromJsonAsync<Response<List<LocationModel>>>(uri);

            return await Task.FromResult(response!);
        }
        catch (Exception ex)
        {
            return new Response<List<LocationModel>>()
            {
                Data = new List<LocationModel>(),
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }
}
