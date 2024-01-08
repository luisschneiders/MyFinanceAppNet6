using MainApp.Components.Chart;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpensePanelRight : ComponentBase
{
    private ChartConfigOption _chartConfigOption { get; set; } = new();
    public AdminExpensePanelRight()
    {
        _chartConfigOption.IndexAxis = "y";
    }
}
