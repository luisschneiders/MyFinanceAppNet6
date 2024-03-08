using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetPanelRight : ComponentBase
{
    // private TimesheetStateContainerDTO _timesheetStateContainerDTO { get; set; } = new();

    public AdminTimesheetPanelRight()
    {
    }

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
