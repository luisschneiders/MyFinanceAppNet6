namespace MainApp.Services;

public interface IGoogleService
{
    Task<Response<List<LocationModel>>> GetGeocodeAddressAsync(string address);
}
