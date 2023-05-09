using Microsoft.AspNetCore.WebUtilities;

namespace MainApp.Services;

public class GoogleService : IGoogleService
{
    private readonly IWebApiService _webApiService;

    public GoogleService(IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }

    public async Task<Response<List<LocationModel>>> GetGeocodeAddressAsync(string address)
    {
        try
        {
            var client = _webApiService.CreateEssentialsHttpClient();

            var query = new Dictionary<string, string>()
            {
                ["Address"] = address
            };

            var uri = QueryHelpers.AddQueryString(EndPoint.V2GoogleGeocode, query!);

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

    public async Task<Response<string>> GetMapStaticImageAsync(GoogleMapStaticModel model)
    {
        try
        {
            var client = _webApiService.CreateEssentialsHttpClient();

            var query = new Dictionary<string, string>()
            {
                ["Location"] = model.Location,
                ["Marker"] = model.Marker,
                ["Scale"] = model.Scale,
                ["Width"] = model.Width.ToString(),
                ["Height"] = model.Height.ToString(),
            };

            var uri = QueryHelpers.AddQueryString(EndPoint.V2GoogleMapStatic, query!);

            Response<byte[]>? response = await client.GetFromJsonAsync<Response<byte[]>>(uri);

            string imgBase64Data = Convert.ToBase64String(response!.Data);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imgBase64Data);

            Response<string> image = new();
            image.Data = imgDataURL;
            image.Success = true;

            return await Task.FromResult(image);
        }
        catch (Exception ex)
        {
            return new Response<string>()
            {
                Data = string.Empty,
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }
}
