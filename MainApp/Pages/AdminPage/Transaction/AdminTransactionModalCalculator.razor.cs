using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionModalCalculator : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    private Modal _modal { get; set; } = new();

    private Guid _modalTarget { get; set; }

    public AdminTransactionModalCalculator()
    {
    }

    public async Task OpenModalAsync()
    {
        try
        {
            _modalTarget = Guid.NewGuid();

            await _modal.Open(_modalTarget);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
