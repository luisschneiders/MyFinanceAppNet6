using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripOffCanvas : ComponentBase
{
    [Inject]
    private ITripService<TripModel> _tripService { get; set; } = default!;

    [Inject]
    private IVehicleService<VehicleModel> _vehicleService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    private TripModel _tripModel { get; set; } = new();
    private List<VehicleModel> _activeVehicles { get; set; } = new();
    private TripCategory[] _tripCategories { get; set; } = default!;
    private List<TripCategoryDTO> _tripCategoryDTOs { get; set; } = new();
    private TripCategoryDTO _tripCategory { get; set; } = new();
    private bool _shouldRender { get; set; } = true;
    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private InputFormAttributes _inputFormAttributes{ get; set; } = new();

    public AdminTripOffCanvas()
    {
        _tripCategories = (TripCategory[])Enum.GetValues(typeof(TripCategory));
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                foreach (var (item, index) in _tripCategories.Select((value, index) => (value, index)))
                {
                    _tripCategory = new() {
                        Id = (ulong)index,
                        Description = _enumHelper.GetDescription(item),
                    };
                    _tripCategoryDTOs.Add(_tripCategory);
                }

                await FetchDataAsync();

                _inputFormAttributes.Control = new()
                {
                    {
                        "class", $"form-control rounded{_appSettings.Form}"
                    }
                };
                _inputFormAttributes.Select = new()
                {
                    {
                        "class", $"form-select rounded{_appSettings.Form}"
                    }
                };
                _inputFormAttributes.PlainText = new()
                {
                    {
                        "class", $"form-control-plaintext"
                    }
                };
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }

        await Task.CompletedTask;
    }

    protected override bool ShouldRender()
    {
        if (_shouldRender)
        {
            Task.FromResult(FetchDataAsync());
        }

        return _shouldRender;
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _tripModel = new()
        {
            TDate = DateTime.Now
        };

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _activeVehicles = await _vehicleService.GetRecordsActive();
            _isLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _displayErrorMessages = false;
            _isProcessing = true;

            await _tripService.CreateRecord(_tripModel);

            _isProcessing = false;
            _toastService.ShowToast("Trip added!", Theme.Success);

            await OnSubmitSuccess.InvokeAsync();
            await Task.Delay((int)Delay.DataSuccess);
            await CloseOffCanvasAsync();

        }
        catch (Exception ex)
        {
            _isProcessing = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleInvalidSubmitAsync()
    {
        _isProcessing = false;
        _displayErrorMessages = true;
        await Task.CompletedTask;
    }

    private async Task ArchiveAsync()
    {
        try
        {
            await _offCanvasService.ArchiveRecordAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateFormStateAsync()
    {
        await _offCanvasService.UpdateFormStateAsync();
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        _tripModel = new();
        _activeVehicles = new();

        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }

    private async void OnValueChangedStartOdometer(ChangeEventArgs args)
    {
        if (decimal.TryParse(args.Value!.ToString(), out decimal result))
        {
            _tripModel.StartOdometer = result;
            CalculateDifference();
        }

        await Task.CompletedTask;
    }

    private async void OnValueChangedEndOdometer(ChangeEventArgs args)
    {
        if (decimal.TryParse(args.Value!.ToString(), out decimal result))
        {
            _tripModel.EndOdometer = result;
            CalculateDifference();
        }

        await Task.CompletedTask;
    }

    private void CalculateDifference()
    {
        _tripModel.Distance = _tripModel.EndOdometer - _tripModel.StartOdometer;
    }
}
