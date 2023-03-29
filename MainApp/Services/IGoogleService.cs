namespace MainApp.Services;

public interface IGoogleService
{
    Task<Response<List<LocationModel>>> GetGeocodingAddressAsync(string address);
}
