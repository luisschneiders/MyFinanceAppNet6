using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionModal : ComponentBase
{
    [Inject]
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    private TransactionModel _transactionModel { get; set; } = new();

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }

    private bool _isProcessing { get; set; } = false;

    public AdminTransactionModal()
    {
    }

    public async Task OpenModalAsync(string id)
    {
        try
        {
            _modalTarget = Guid.NewGuid();
            _transactionModel = await _transactionService.GetRecordById(id);

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
        _transactionModel = new();
        _isProcessing = false;

        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _isProcessing = true;

            await _transactionService.ArchiveRecord(_transactionModel);

            _toastService.ShowToast("Transaction archived!", Theme.Success);

            await OnSubmitSuccess.InvokeAsync();
            await Task.Delay((int)Delay.DataSuccess);
            await CloseModalAsync();
        }
        catch (Exception ex)
        {
            _isProcessing = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleInvalidSubmitAsync()
    {
        _isProcessing = false;
        await Task.CompletedTask;
    }
}
