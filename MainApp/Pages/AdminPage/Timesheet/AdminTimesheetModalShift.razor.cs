using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetModalShift : ComponentBase
{
    [Inject]
    private IShiftService<ShiftModel> _shiftService { get; set; } = default!;

    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;

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

    private List<CompanyModel> _activeCompanies { get; set; } = new();

    private Dictionary<string, object> _inputFormSelectAttributes = default!;
    private Dictionary<string, object> _inputFormControlAttributes = default!;

    private bool _displayErrorMessages { get; set; } = false;

    private bool _isProcessing { get; set; } = false;

    private bool _isLoading { get; set; } = true;

    public AdminTimesheetModalShift()
    {
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
                        "class", $"form-control rounded{_appSettings.Form}"
                    }
                };

                _inputFormSelectAttributes = new()
                {
                    {
                        "class", $"form-select rounded{_appSettings.Form}"
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


    public async Task OpenModalAsync(DateTime date)
    {
        try
        {
            _modalTarget = Guid.NewGuid();
            _dateTimeRange.Start = date;
            _dateTimeRange.End = date;

            _shiftModel = new()
            {
                SDate = date
            };

            await _modal.Open(_modalTarget);

            await FetchDataAsync();

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
        await ResetAsync();
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task ResetAsync()
    {
        _activeCompanies = new();

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _activeCompanies = await _companyService.GetRecordsActive();
            _isLoading = false;
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

            await _shiftService.SaveRecord(_shiftModel);
            await Task.Delay((int)Delay.DataSuccess);

            _isProcessing = false;

            await OnSubmitSuccess.InvokeAsync();

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
