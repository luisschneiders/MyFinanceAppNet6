using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpenseOffCanvas : ComponentBase
{
    [Inject]
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private IBankService<BankModel> _bankService { get; set; } = default!;

    [Inject]
    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    private ExpenseModel _expenseModel { get; set; } = new();
    private List<BankModel> _activeBanks { get; set; } = new();
    private List<ExpenseCategoryModel> _activeExpenseCategories { get; set; } = new();

    private bool _shouldRender { get; set; } = true;
    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;
    private bool _isLoading { get; set; } = true;

    public AdminExpenseOffCanvas()
    {
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await FetchDataAsync();
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }

        await Task.CompletedTask;
    }

    protected override bool ShouldRender()
    {
        if (_shouldRender)
        {
            Task.FromResult(FetchDataAsync());
        }

        return _shouldRender;
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _expenseModel = new();
        _expenseModel.EDate = DateTime.Today;

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _activeBanks = await _bankService.GetRecordsActive();
            _activeExpenseCategories = await _expenseCategoryService.GetRecordsActive();
            _isLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async void OnValueChangedBank(ChangeEventArgs args)
    {
        var valueChanged = args?.Value;

        if (valueChanged is not null)
        {
            ulong bankId = ulong.Parse(valueChanged.ToString()!);

            if (bankId != 0)
            {
                _expenseModel.BankModel = _activeBanks.Find(c => c.Id == bankId);
            }
            else
            {
                _expenseModel.BankModel = new();
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }

    private async void OnValueChangedExpenseCategory(ChangeEventArgs args)
    {
        var valueChanged = args?.Value;

        if (valueChanged is not null)
        {
            ulong categotyId = ulong.Parse(valueChanged.ToString()!);

            if (categotyId != 0)
            {
                _expenseModel.ExpenseCategoryModel = _activeExpenseCategories.Find(c => c.Id == categotyId);
            }
            else
            {
                _expenseModel.ExpenseCategoryModel = new();
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _displayErrorMessages = false;
            _isProcessing = true;

            await _expenseService.CreateRecord(_expenseModel);

            _isProcessing = false;
            _toastService.ShowToast("Expense added!", Theme.Success);

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

    private async Task UpdateFormStateAsync()
    {
        await _offCanvasService.UpdateFormStateAsync();
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        _expenseModel = new();
        _activeBanks = new();

        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }
}
