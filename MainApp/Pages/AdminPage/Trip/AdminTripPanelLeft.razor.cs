using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripPanelLeft : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    private DateTimeRangeModel _dateTimeRangeModel { get; set; } = new();

    public AdminTripPanelLeft()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRangeModel = _dateTimeService.GetCurrentMonth();
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        //await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshListFromDropdownDateRange()
    {
        //await FetchDataAsync();
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await Task.CompletedTask;
    }
}
