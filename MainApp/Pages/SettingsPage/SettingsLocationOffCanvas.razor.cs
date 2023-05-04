using System.Collections.Generic;
using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsLocationOffCanvas : ComponentBase
{
    [Inject]
    private IGoogleService _googleService { get; set; } = default!;

    [Inject]
    private ILocationService<UserLocationModel> _locationService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private OffCanvas _offCanvas { get; set; } = new();
    private string _offCanvasTarget { get; set; } = string.Empty;

    private LocationModel _locationModel { get; set; } = new();
    private LocationModel _currentLocation { get; set; } = new();
    private UserLocationModel _userLocationModel { get; set; } = new();
    private List<LocationModel> _locationlist { get; set; } = new();

    private bool _isProcessing { get; set; } = false;
    private bool _isVerifying { get; set; } = false;
    private bool _formIsInvalid { get; set; } = false;
    private bool _userLocationIsInvalid { get; set; } = false;

    public SettingsLocationOffCanvas()
    {
    }

    public async Task OpenAsync()
    {
        _offCanvasTarget = Guid.NewGuid().ToString();

        await ResetDefaults();
        await FetchDataAsync();
        await Task.FromResult(_offCanvas.Open(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        await Task.FromResult(_offCanvas.Close(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task ResetDefaults()
    {
        _locationModel = new();
        _currentLocation = new();
        _userLocationModel = new();
        _locationlist = new();
        _formIsInvalid = false;
        _userLocationIsInvalid = false;
        _isProcessing = false;

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _currentLocation = await _locationService.GetRecordById();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task VerifyAddress()
    {
        try
        {
            _formIsInvalid = false;

            if (string.IsNullOrWhiteSpace(_locationModel.Address))
            {
                _formIsInvalid = true;
                return;
            }

            _isVerifying = true;

            Response<List<LocationModel>> response = await _googleService.GetGeocodeAddressAsync(_locationModel.Address);

            if (response.Success)
            {
                _locationlist = response.Data;
            }
            else
            {
                _locationlist = new();
                _toastService.ShowToast(response.ErrorMessage, Theme.Danger);
            }

            _isVerifying = false;

        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task SelectAddress(LocationModel location)
    {
        _userLocationModel.Location = location;
        await Task.CompletedTask;
    }

    private async Task SaveAddress()
    {
        try
        {

            _isProcessing = true;

            if (_userLocationModel.Location.Latitude == 0 && _userLocationModel.Location.Longitude == 0)
            {
                _userLocationIsInvalid = true;
                _isProcessing = false;

                return;
            }

            await _locationService.SaveRecord(_userLocationModel);

            _toastService.ShowToast("Location saved!", Theme.Success);

            await Task.Delay((int)Delay.DataSuccess);
            await ResetDefaults();
            await CloseOffCanvasAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }
}
