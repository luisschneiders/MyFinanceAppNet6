using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage;

public partial class SetupBankOffCanvas
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    IBankService _bankService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    public BankModel BankModel { get; set; } = new();

    private OffCanvas _offCanvas { get; set; } = default!;
    private OffCanvasViewType _offCanvasViewType { get; set; }
    private string _offCanvasTarget { get; set; } = string.Empty;
    private Theme _badgeBackground { get; set; } = Theme.Info;
    private bool _displayValidationErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    private BankModel _bankModel { get; set; } = new();

    public SetupBankOffCanvas()
    {
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _bankModel = new();

        await Task.FromResult(SetOffCanvasState(OffCanvasViewType.Add, Theme.Success));

        await Task.FromResult(_offCanvas.Open(Guid.NewGuid().ToString()));
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        await Task.FromResult(SetOffCanvasState(OffCanvasViewType.Edit, Theme.Danger));
        await Task.FromResult(SetOffCanvasInfo(id));

        await Task.FromResult(_offCanvas.Open(_offCanvasTarget));

        await Task.CompletedTask;
    }

    public async Task ViewRecordOffCanvasAsync(string id)
    {
        await Task.FromResult(SetOffCanvasState(OffCanvasViewType.View, Theme.Info));
        await Task.FromResult(SetOffCanvasInfo(id));

        await Task.FromResult(_offCanvas.Open(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task SetOffCanvasState(OffCanvasViewType offCanvasViewType, Theme theme)
    {
        _offCanvasViewType = offCanvasViewType;
        _badgeBackground = theme;

        await Task.CompletedTask;
    }

    private async Task SetOffCanvasInfo(string id)
    {
        _offCanvasTarget = id;
        _bankModel = await _bankService.GetBankById(id);

        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        _bankModel = new();

        await Task.FromResult(_offCanvas.Close(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task UpdateFormState(OffCanvasViewType offCanvasViewType, Theme theme)
    {
        await Task.FromResult(SetOffCanvasState(offCanvasViewType, theme));
        await Task.CompletedTask;
    }

    private async Task ArchiveRecordAsync()
    {
        try
        {
            await Task.FromResult(SetOffCanvasState(OffCanvasViewType.Archive, Theme.Danger));
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
        _displayValidationErrorMessages = false;
        _isProcessing = true;

        if (_offCanvasViewType == OffCanvasViewType.Add)
        {

            // Set the initial balance to the same as the current balance for new records
            _bankModel.InitialBalance = _bankModel.CurrentBalance;

            await _bankService.CreateBank(_bankModel);

            _bankModel.Id = await _bankService.GetLastInsertedId();
            _toastService.ShowToast("Bank added!", Theme.Success);
        }
        else if (_offCanvasViewType == OffCanvasViewType.Edit)
        {
            await _bankService.UpdateBank(_bankModel);
            _toastService.ShowToast("Bank updated!", Theme.Success);
        }
        else if (_offCanvasViewType == OffCanvasViewType.Archive)
        {
            await _bankService.ArchiveBank(_bankModel);
            _toastService.ShowToast("Bank archived!", Theme.Success);
        }

        _isProcessing = false;

        BankModel = _bankModel;

        await OnSubmitSuccess.InvokeAsync();

        await Task.Delay((int)Delay.DataSuccess);

        await CloseOffCanvasAsync();
        await Task.CompletedTask;

    }

    private async Task HandleInvalidSubmitAsync()
    {
        _isProcessing = false;
        _displayValidationErrorMessages = true;
        await Task.CompletedTask;
    }

}
