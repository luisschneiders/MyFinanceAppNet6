namespace MainApp.Components.Map;

public interface IMapService
{
    Task InitializeMap(string mapId, LocationModel location, int zoom);
    Task AddMarker(string mapId, List<LocationModel> locations);
    Task FitMarkerToView(string mapId, List<LocationModel> locations);
}
