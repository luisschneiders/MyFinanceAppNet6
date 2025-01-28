namespace MainApp.Services;

public interface IGoogleService
{
    public Task<Response<List<LocationModel>>> GetGeocodeAddress(string address);
    public Task<Response<string>> GetMapStaticImage(GoogleMapStaticModel model);
    public Task<Response<Uri>> GetMapInteractiveUrl();
}
