namespace MainApp.Services;

public interface IGoogleService
{
    Task<Response<List<LocationModel>>> GetGeocodeAddress(string address);
    Task<Response<string>> GetMapStaticImage(GoogleMapStaticModel model);
}
