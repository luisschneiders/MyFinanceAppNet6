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
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private WeatherModel _weatherModel { get; set; } = default!;

    private bool _isWeatherAvailable { get; set; } = false;
    private bool _isNight { get; set; } = false;

    public WeatherCondition()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await FetchDataAsync();
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
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

                    _isWeatherAvailable = true;
                    _weatherModel = response.Data;

                    _isNight = _dateTimeService.CheckIsNight(_weatherModel.LocalTime);

                    StateHasChanged();
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
}
