using MainApp.Pages.AdminPage.Timesheet;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionPanelLeft : ComponentBase
{
    /*
     * Add OffCanvas component reference
     */
    private AdminTransactionOffCanvas _setupOffCanvas { get; set; } = new();

    public AdminTransactionPanelLeft()
    {
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        //await FetchDataAsync();
        await Task.CompletedTask;
    }
}
