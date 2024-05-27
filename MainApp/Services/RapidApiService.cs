﻿using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;

namespace MainApp.Services;

public class RapidApiService : IRapidApiService
{
    private readonly IEssentialsAPIService _essentialsAPIService;

    public RapidApiService(IEssentialsAPIService essentialsAPIService)
    {
        _essentialsAPIService = essentialsAPIService;
    }

    public async Task<Response<WeatherModel>> GetWeatherCondition(LocationModel locationModel)
    {
        try
        {
            var client = _essentialsAPIService.CreateHttpClient();

            // Retrieve token for authorization
            string token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
                ErrorMessage = "Rapid API says: " + ex.Message,
            };
        }
    }

    private async Task<string> GetToken()
    {
        Response<string> response = await _essentialsAPIService.GetTokenWithBasicAuthAsync();
        return await Task.FromResult(response.Data);
    }
}
