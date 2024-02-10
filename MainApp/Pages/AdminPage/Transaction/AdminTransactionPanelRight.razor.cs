using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionPanelRight : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    /*
     * Add Chart Modal component reference
     */
    private AdminTransactionChartModal _setupChartModal { get; set; } = new();

    public AdminTransactionPanelRight()
    {
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
