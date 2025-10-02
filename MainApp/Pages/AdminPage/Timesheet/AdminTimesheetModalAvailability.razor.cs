
using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetModalAvailability : ComponentBase
{
    [Inject]
    private IShiftService<ShiftModel> _shiftService { get; set; } = default!;
    [Inject]
    private ToastService _toastService { get; set; } = default!;


    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public EventCallback<DateTime> OnSubmitSuccess { get; set; }

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private DateTimeRange _dateTimeRange { get; set; } = new();
    private ShiftModel _shiftModel { get; set; } = new();
    private InputFormAttributes _inputFormAttributes { get; set; } = new();
    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    public AdminTimesheetModalAvailability()
    {
    }

    public async Task OpenModalAsync(DateTime date)
    {
        try
        {
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

            _modalTarget = Guid.NewGuid();
            _dateTimeRange.Start = date;
            _dateTimeRange.End = date;

            _shiftModel = new()
            {
                SDate = date
            };

            await _modal.Open(_modalTarget);

            // await FetchDataAsync();

            // await FetchShiftDataAsync();

            await InvokeAsync(StateHasChanged);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _isProcessing = true;
            _displayErrorMessages = false;

            await _shiftService.SaveAvailability(_shiftModel);
            await Task.Delay((int)Delay.DataSuccess);

            // await FetchShiftDataAsync();
            await OnSubmitSuccess.InvokeAsync();
            _isProcessing = false;

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
}
