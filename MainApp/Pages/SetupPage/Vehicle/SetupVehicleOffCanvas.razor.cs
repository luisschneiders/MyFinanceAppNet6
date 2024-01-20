using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage.Vehicle;

public partial class SetupVehicleOffCanvas : ComponentBase
{
    [Inject]
    private IVehicleService<VehicleModel> _vehicleService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public VehicleModel DataModel { get; set; } = default!;

    [Parameter]
    public AppSettings AppSettings { get; set; } = default!;

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    private Dictionary<string, object> _inputFormControlAttributes = default!;
    private Dictionary<string, object> _inputFormControlPlainTextAttributes = default!;

    private VehicleModel _vehicleModel { get; set; } = new();

    private DateTime _currentYear { get; }

    public SetupVehicleOffCanvas()
    {
        _currentYear = DateTime.Now;
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _inputFormControlAttributes = new()
                {
                    {
                        "class", $"form-control rounded{AppSettings.Form}"
                    }
                };
                _inputFormControlPlainTextAttributes = new()
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

    public async Task AddRecordOffCanvasAsync()
    {
        _vehicleModel = new();

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        try
        {
            _vehicleModel = await _vehicleService.GetRecordById(id);
            if (_vehicleModel is not null)
            {
                await _offCanvasService.EditRecordAsync(id);
            }
            else
            {
                _vehicleModel = new();
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    public async Task ViewRecordOffCanvasAsync(string id)
    {
        try
        {
            _vehicleModel = await _vehicleService.GetRecordById(id);
            if (_vehicleModel is not null)
            {
                await _offCanvasService.ViewRecordAsync(id);
            }
            else
            {
                _vehicleModel = new();
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
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

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _displayErrorMessages = false;
            _isProcessing = true;

            var offCanvasViewType = _offCanvasService.GetOffCanvasViewType();

            if (offCanvasViewType == OffCanvasViewType.Add)
            {
                await _vehicleService.CreateRecord(_vehicleModel);

                _vehicleModel.Id = await _vehicleService.GetLastInsertedId();
                _toastService.ShowToast("Vehicle added!", Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Edit)
            {
                await _vehicleService.UpdateRecord(_vehicleModel);
                _toastService.ShowToast("Vehicle updated!", Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Archive)
            {
                await _vehicleService.ArchiveRecord(_vehicleModel);
                _toastService.ShowToast("Vehicle archived!", Theme.Success);
            }
            _isProcessing = false;

            DataModel = _vehicleModel;

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

    private async Task CloseOffCanvasAsync()
    {
        _vehicleModel = new();
        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }
}
