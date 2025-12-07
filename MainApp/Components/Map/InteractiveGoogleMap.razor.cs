using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Map;

public partial class InteractiveGoogleMap : ComponentBase
{
    [Inject]
    private ILocationService<UserLocationModel> _locationService { get; set; } = default!;

    [Inject]
    private IMapService _mapService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();


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
            try
            {
                _currentLocation = await _locationService.GetRecordById();

                if (_currentLocation is null)
                {
                    throw new InvalidOperationException(Label.AppComponentInteractiveMapLocationNotFound);
                }

                await _mapService.InitializeMap(Label.AppMapInteractiveGoogle, _currentLocation, 12);
                await _mapService.AddMarker(Label.AppMapInteractiveGoogle, Locations);
                await _mapService.FitMarkerToView(Label.AppMapInteractiveGoogle, Locations);
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

        }

        await Task.CompletedTask;
    }
}
