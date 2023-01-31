using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage.Expense;

public partial class SetupExpenseOffCanvas : ComponentBase
{
    [Inject]
    private IExpenseService _expenseService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public ExpenseModel DataModel { get; set; } = default!;

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    private ExpenseModel _expenseModel { get; set; } = new();

    public SetupExpenseOffCanvas()
    {
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _expenseModel = new();

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        try
        {
            _expenseModel = await _expenseService.GetExpenseById(id);
            if (_expenseModel is not null)
            {
                await _offCanvasService.EditRecordAsync(id);
            }
            else
            {
                _expenseModel = new();
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    public async Task ViewRecordOffCanvasAsync(string id)
    {
        try
        {
            _expenseModel = await _expenseService.GetExpenseById(id);
            if (_expenseModel is not null)
            {
                await _offCanvasService.ViewRecordAsync(id);
            }
            else
            {
                _expenseModel = new();
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
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
            await _expenseService.CreateExpense(_expenseModel);

            _expenseModel.Id = await _expenseService.GetLastInsertedId();
            _toastService.ShowToast("Expense added!", Theme.Success);
        }
        else if (offCanvasViewType == OffCanvasViewType.Edit)
        {
            await _expenseService.UpdateExpense(_expenseModel);
            _toastService.ShowToast("Expense updated!", Theme.Success);
        }
        else if (offCanvasViewType == OffCanvasViewType.Archive)
        {
            await _expenseService.ArchiveExpense(_expenseModel);
            _toastService.ShowToast("Expense archived!", Theme.Success);
        }

        _isProcessing = false;

        DataModel = _expenseModel;

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
        _expenseModel = new();

        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }

}
