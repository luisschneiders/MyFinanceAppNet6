using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class InteractiveMap : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    private List<LocationModel> _locations = new();

    public InteractiveMap()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _locations.Add(new LocationModel()
                    {
                        Id = "1",
                        Address = "Timboon VIC 3268, Australia",
                        Latitude = (decimal)-38.484271,
                        Longitude = (decimal)142.979056,
                    });
                _locations.Add(new LocationModel()
                    {
                        Id = "2",
                        Address = "Warrnambool VIC 3280, Australia",
                        Latitude = (decimal)-38.372095,
                        Longitude = (decimal)142.477778,
                    });
                _locations.Add(new LocationModel()
                    {
                        Id = "3",
                        Address = "Geelong VIC 3220, Australia",
                        Latitude = (decimal)-38.149299,
                        Longitude = (decimal)144.359843,
                    });
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }
}
