using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetModalCalculator : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private InputFormAttributes _inputFormAttributes{ get; set; } = new();

    public AdminTimesheetModalCalculator()
    {
    }

    public async Task OpenModalAsync()
    {
        try
        {
            _inputFormAttributes.Control = new()
            {
                {
                    "class", $"form-control rounded{_appSettings.Form}"
                }
            };

            _modalTarget = Guid.NewGuid();

            await _modal.Open(_modalTarget);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
