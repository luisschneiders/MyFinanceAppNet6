using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

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

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    private TripModel _tripModel { get; set; } = new();
    private List<VehicleModel> _activeVehicles { get; set; } = new();

    private bool _shouldRender { get; set; } = true;
    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;
    private bool _isLoading { get; set; } = true;

    public AdminTripOffCanvas()
    {
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
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
        _tripModel = new();
        _tripModel.TDate = DateTime.Now;

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
}
