using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage;

public partial class SetupBankOffCanvas
{

    [Inject]
    private IBankService _bankService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public BankModel DataModel { get; set; } = default!;

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    private BankModel _bankModel { get; set; } = new();

    public SetupBankOffCanvas()
    {
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _bankModel = new();

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        _bankModel = await _bankService.GetBankById(id);

        await _offCanvasService.EditRecordAsync(id);
        await Task.CompletedTask;
    }

    public async Task ViewRecordOffCanvasAsync(string id)
    {
        _bankModel = await _bankService.GetBankById(id);

        await _offCanvasService.ViewRecordAsync(id);
        await Task.CompletedTask;
    }

    private async Task UpdateFormStateAsync()
    {
        await _offCanvasService.UpdateFormStateAsync();
        await Task.CompletedTask;
    }

    private async Task ArchiveAsync()
    {
        try
        {
            await _offCanvasService.ArchiveRecordAsync();
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        _displayErrorMessages = false;
        _isProcessing = true;

        var offCanvasViewType = _offCanvasService.GetOffCanvasViewType();

        if (offCanvasViewType == OffCanvasViewType.Add)
        {
            // Set the initial balance equal to current balance for new records
            _bankModel.InitialBalance = _bankModel.CurrentBalance;

            await _bankService.CreateBank(_bankModel);

            _bankModel.Id = await _bankService.GetLastInsertedId();
            _toastService.ShowToast("Bank added!", Theme.Success);
        }
        else if (offCanvasViewType == OffCanvasViewType.Edit)
        {
            await _bankService.UpdateBank(_bankModel);
            _toastService.ShowToast("Bank updated!", Theme.Success);
        }
        else if (offCanvasViewType == OffCanvasViewType.Archive)
        {
            await _bankService.ArchiveBank(_bankModel);
            _toastService.ShowToast("Bank archived!", Theme.Success);
        }

        _isProcessing = false;

        DataModel = _bankModel;

        await OnSubmitSuccess.InvokeAsync();

        await Task.Delay((int)Delay.DataSuccess);

        await CloseOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task HandleInvalidSubmitAsync()
    {
        _isProcessing = false;
        _displayErrorMessages = true;
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        _bankModel = new();

        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }
}
