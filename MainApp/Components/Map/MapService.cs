using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.Map;

public class MapService : IMapService
{
    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    public MapService(IJSRuntime jSRuntime, IGoogleService googleService)
    {
        _jsRuntime = jSRuntime;
    }

    public async Task InitializeMap(string mapId, LocationModel location, int zoom)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("initializeGoogleMap", mapId, new { lat = location.Latitude, lng = location.Longitude }, zoom);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task AddMarker(string mapId, List<LocationModel> locations)
    {
        try
        {
            foreach (var (position, index) in locations.Select((pos, i) => (pos, i)))
            {
                await _jsRuntime.InvokeVoidAsync("addGoogleMapMarker", mapId, $"{position.Address}", new { lat = position.Latitude, lng = position.Longitude });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task FitMarkerToView(string mapId, List<LocationModel> locations)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("fitGoogleMapMarkerToView", mapId, locations);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
