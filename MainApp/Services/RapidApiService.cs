using Microsoft.AspNetCore.WebUtilities;

namespace MainApp.Services;

public class RapidApiService : IRapidApiService
{
    private readonly IWebApiService _webApiService;

    public RapidApiService(IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }

    public async Task<Response<WeatherModel>> GetWeatherCondition(LocationModel locationModel)
    {
        try
        {
            var client = _webApiService.CreateEssentialsHttpClient();

            var query = new Dictionary<string, string>()
            {
                ["Latitude"] = locationModel.Latitude.ToString(),
                ["Longitude"] = locationModel.Longitude.ToString(),
            };

            var uri = QueryHelpers.AddQueryString(EndPoint.V2RapidApiWeatherCondition, query!);

            Response<WeatherModel>? response = await client.GetFromJsonAsync<Response<WeatherModel>>(uri);

            return await Task.FromResult(response!);
        }
        catch (Exception ex)
        {
            return new Response<WeatherModel>()
            {
                Data = new WeatherModel(),
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }
}
