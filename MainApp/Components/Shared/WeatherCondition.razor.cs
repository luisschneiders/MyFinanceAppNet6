using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Shared;

public partial class WeatherCondition : ComponentBase
{
    [Inject]
    private ILocationService<UserLocationModel> _locationService { get; set; } = default!;

    [Inject]
    private IRapidApiService _rapidApiService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private WeatherModel _weatherModel { get; set; } = default!;

    private bool _isWeatherAvailable { get; set; } = false;
    private bool _isNight { get; set; } = false;

    public WeatherCondition()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        try
        {
            await FetchDataAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            LocationModel locationModel = await _locationService.GetRecordById();

            if (locationModel is not null)
            {
                Response<WeatherModel> response = await _rapidApiService.GetWeatherCondition(locationModel);

                if (response.Success)
                {
                    StateHasChanged();
                    _isWeatherAvailable = true;
                    _weatherModel = response.Data;

                    var isNight = await IsNight(_weatherModel.LocalTime);
                    _isNight = isNight;
                }
                else
                {
                    _weatherModel = new();
                    _toastService.ShowToast(response.ErrorMessage, Theme.Danger);
                    _isWeatherAvailable = false;
                }
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private static async Task<bool> IsNight(string localTime)
    {
        DateTime dateTime = DateTime.Parse(localTime);

        return await Task.FromResult(dateTime.Hour >= 18 || dateTime.Hour < 6);
    }
}
