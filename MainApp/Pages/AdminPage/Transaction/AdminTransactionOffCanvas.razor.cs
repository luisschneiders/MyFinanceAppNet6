using MainApp.Components.OffCanvas;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionOffCanvas : ComponentBase
{
    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    public AdminTransactionOffCanvas()
    {
    }
}
