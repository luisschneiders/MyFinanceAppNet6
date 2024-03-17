using MainApp.Components.Chart;
using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpenseChartModal : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private ChartConfigOption _chartConfigOption { get; set; } = new();

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private DateTimeRange _dateTimeRange { get; set; } = new();

    public AdminExpenseChartModal()
    {
        _chartConfigOption.IndexAxis = "y";
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        await Task.CompletedTask;
    }

    public async Task OpenModalAsync()
    {
        try
        {
            _modalTarget = Guid.NewGuid();

            await Task.FromResult(_modal.Open(_modalTarget));
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
}
