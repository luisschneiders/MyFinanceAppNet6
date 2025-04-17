using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetPanelRight : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private InputFormAttributes _inputFormAttributes{ get; set; } = new();

    public AdminTimesheetPanelRight()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _inputFormAttributes.Control = new()
            {
                {
                    "class", $"form-control rounded{_appSettings.Form}"
                }
            };


            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;    }

    // protected override async Task OnInitializedAsync()
    // {
    //     _timesheetStateService.OnStateChange += StateHasChanged;

    //     _timesheetStateContainerDTO = _timesheetStateService.Value;

    //     await Task.CompletedTask;
    // }

    // protected override async Task OnAfterRenderAsync(bool firstRender)
    // {
    //     if (firstRender)
    //     {
    //         try
    //         {
    //             _spinnerService.ShowSpinner();
    //             _isLoading = false;
    //         }
    //         catch (Exception ex)
    //         {
    //             _isLoading = false;
    //             _toastService.ShowToast(ex.Message, Theme.Danger);
    //         }

    //         StateHasChanged();
    //     }

    //     await Task.CompletedTask;
    // }

    // public void Dispose()
    // {
    //     _timesheetStateService.OnStateChange -= StateHasChanged;
    // }
}
