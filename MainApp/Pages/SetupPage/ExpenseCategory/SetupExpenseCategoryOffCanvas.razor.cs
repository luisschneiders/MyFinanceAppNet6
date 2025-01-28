using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage.ExpenseCategory;

public partial class SetupExpenseCategoryOffCanvas : ComponentBase
{
    [Inject]
    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public ExpenseCategoryModel DataModel { get; set; } = default!;

    [Parameter]
    public IAppSettings AppSettings { get; set; } = default!;

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    private Dictionary<string, object> _inputFormControlAttributes = default!;
    private Dictionary<string, object> _inputFormControlPlainTextAttributes = default!;

    private ExpenseCategoryModel _expenseCategoryModel { get; set; } = new();

    public SetupExpenseCategoryOffCanvas()
    {
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _inputFormControlAttributes = new()
                {
                    {
                        "class", $"form-control rounded{AppSettings.Form}"
                    }
                };
                _inputFormControlPlainTextAttributes = new()
                {
                    {
                        "class", $"form-control-plaintext"
                    }
                };
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }

        await Task.CompletedTask;
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _expenseCategoryModel = new();
        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        try
        {
            _expenseCategoryModel = await _expenseCategoryService.GetRecordById(id);
            if (_expenseCategoryModel is not null)
            {
                await _offCanvasService.EditRecordAsync(id);
            }
            else
            {
                _expenseCategoryModel = new();
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    public async Task ViewRecordOffCanvasAsync(string id)
    {
        try
        {
            _expenseCategoryModel = await _expenseCategoryService.GetRecordById(id);
            if (_expenseCategoryModel is not null)
            {
                await _offCanvasService.ViewRecordAsync(id);
            }
            else
            {
                _expenseCategoryModel = new();
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
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
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _displayErrorMessages = false;
            _isProcessing = true;

            var offCanvasViewType = _offCanvasService.GetOffCanvasViewType();

            if (offCanvasViewType == OffCanvasViewType.Add)
            {
                await _expenseCategoryService.CreateRecord(_expenseCategoryModel);

                _expenseCategoryModel.Id = await _expenseCategoryService.GetLastInsertedId();
                _toastService.ShowToast("Expense category added!", Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Edit)
            {
                await _expenseCategoryService.UpdateRecord(_expenseCategoryModel);
                _toastService.ShowToast("Expense category updated!", Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Archive)
            {
                await _expenseCategoryService.ArchiveRecord(_expenseCategoryModel);
                _toastService.ShowToast("Expense category archived!", Theme.Success);
            }

            _isProcessing = false;

            DataModel = _expenseCategoryModel;

            await OnSubmitSuccess.InvokeAsync();

            await Task.Delay((int)Delay.DataSuccess);

            await CloseOffCanvasAsync();
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
        _displayErrorMessages = true;
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        _expenseCategoryModel = new();
        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }
}
