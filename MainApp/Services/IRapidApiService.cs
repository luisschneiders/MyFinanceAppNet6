namespace MainApp.Services;

public interface IRapidApiService
{
    Task<Response<WeatherModel>> GetWeatherCondition(LocationModel locationModel);
}
