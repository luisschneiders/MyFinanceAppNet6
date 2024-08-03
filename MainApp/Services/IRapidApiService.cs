namespace MainApp.Services;

public interface IRapidApiService
{
    public Task<Response<WeatherModel>> GetWeatherCondition(LocationModel locationModel);
}
