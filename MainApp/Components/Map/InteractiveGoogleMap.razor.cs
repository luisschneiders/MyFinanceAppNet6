using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Map;

public partial class InteractiveGoogleMap : ComponentBase
{
    [Inject]
    private ILocationService<UserLocationModel> _locationService { get; set; } = default!;

    [Inject]
    private IMapService _mapService { get; set; } = default!;

    private LocationModel _currentLocation { get; set; } = new();

    [Parameter]
    public List<LocationModel> Locations { get; set; } = new();

    public InteractiveGoogleMap()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _currentLocation = await _locationService.GetRecordById();

            await _mapService.InitializeMap(Label.MapInteractiveGoogle, _currentLocation, 12);
            await _mapService.AddMarker(Label.MapInteractiveGoogle, Locations);
            await _mapService.FitMarkerToView(Label.MapInteractiveGoogle, Locations);
        }

        await Task.CompletedTask;
    }
}
