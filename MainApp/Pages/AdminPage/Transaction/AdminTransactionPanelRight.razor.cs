using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionPanelRight : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    public AdminTransactionPanelRight()
    {
    }
 }
