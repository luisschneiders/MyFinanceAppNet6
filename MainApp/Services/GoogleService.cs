using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;

namespace MainApp.Services;

public class GoogleService : IGoogleService
{
    private readonly IEssentialsAPIService _essentialsAPIService;
    private string _token { get; set;} = string.Empty;
    public GoogleService(IEssentialsAPIService essentialsAPIService)
    {
        _essentialsAPIService = essentialsAPIService;
    }

    public async Task<Response<List<LocationModel>>> GetGeocodeAddress(string address)
    {
        try
        {
            var client = _essentialsAPIService.CreateHttpClient();

            // Retrieve token for authorization
            string token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
                ErrorMessage = "Google says: " + ex.Message,
            };
        }
    }

    public async Task<Response<string>> GetMapStaticImage(GoogleMapStaticModel model)
    {
        try
        {
            var scale = (int)model.Scale;
            var width = (int)model.Width;
            var height = (int)model.Height;

            var client = _essentialsAPIService.CreateHttpClient();

            // Retrieve token for authorization
            string token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var query = new Dictionary<string, string>()
            {
                ["Location"] = model.Location,
                ["Marker"] = model.Marker,
                ["Scale"] = scale.ToString(),
                ["Width"] = width.ToString(),
                ["Height"] = height.ToString(),
            };

            var uri = QueryHelpers.AddQueryString(EndPoint.V2GoogleStaticMapImage, query!);

            Response<byte[]>? response = await client.GetFromJsonAsync<Response<byte[]>>(uri);

            string imgBase64Data = Convert.ToBase64String(response!.Data);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imgBase64Data);

            Response<string> image = new()
            {
                Data = imgDataURL,
                Success = true
            };

            return await Task.FromResult(image);
        }
        catch (Exception ex)
        {
            return new Response<string>()
            {
                Data = string.Empty,
                Success = false,
                ErrorMessage = "Google says: " + ex.Message,
            };
        }
    }

    private async Task<string> GetToken()
    {
        Response<string> response = await _essentialsAPIService.GetTokenWithBasicAuthAsync();
        return await Task.FromResult(response.Data);
    }
}
