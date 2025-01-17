using MainApp.Components.Chart;
using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpenseChart : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private IBrowserService _browserService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private ChartConfigOption _chartConfigOption { get; set; } = new();
    private DateTimeRange _dateTimeRange { get; set; } = new();

    public AdminExpenseChart()
    {
        _chartConfigOption.IndexAxis = "y";
    }

    protected async override Task OnInitializedAsync()
    {
        _modalTarget = Guid.NewGuid();
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        await Task.CompletedTask;
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await Task.FromResult(_modal.Open(_modalTarget));
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            StateHasChanged();
        }
        await Task.CompletedTask;
    }

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_browserService.CloseTab());
        await Task.CompletedTask;
    }
}
