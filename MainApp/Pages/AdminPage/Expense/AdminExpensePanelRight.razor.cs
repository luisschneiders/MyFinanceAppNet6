using MainApp.Components.Chart;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpensePanelRight : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    /*
     * Add Chart Modal component reference
     */
    private AdminExpenseChartModal _setupChartModal { get; set; } = new();

    private ChartConfigOption _chartConfigOption { get; set; } = new();
    public AdminExpensePanelRight()
    {
        _chartConfigOption.IndexAxis = "y";
    }

    private async Task ViewChartsAsync()
    {
        try
        {
            await _setupChartModal.OpenModalAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }
}
