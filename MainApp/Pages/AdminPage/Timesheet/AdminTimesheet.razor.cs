using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheet : ComponentBase
{
    private bool _isLoading { get; set; } = true;

    public AdminTimesheet()
    {
    }

    private async Task DisplayPanelRight()
    {
        _isLoading = false;
        await Task.CompletedTask;
    }
}
