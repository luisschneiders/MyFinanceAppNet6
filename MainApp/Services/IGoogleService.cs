namespace MainApp.Services;

public interface IGoogleService
{
    Task<Response<List<LocationModel>>> GetGeocodeAddressAsync(string address);
    Task<Response<string>> GetMapStaticImageAsync(GoogleMapStaticModel model);
}
