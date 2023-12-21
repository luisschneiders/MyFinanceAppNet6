using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpenseFilterModal : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDropdownFilterService _dropdownFilterService { get; set; } = default!;

    [Inject]
    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;

    [Inject]
    private IBankService<BankModel> _bankService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback<FilterExpenseDTO> OnSubmitFilterSuccess { get; set; }

    private FilterModel _filterExpenseCategoryModel { get; set; } = new();
    private FilterModel _filterBankModel { get; set; } = new();
    private FilterExpenseDTO _filterExpenseDTO { get; set; } = new();
    private ExpenseCategoryModel _filterExpenseCategory { get; set; } = new();
    private BankModel _filterBank { get; set; } = new();
    private List<ExpenseCategoryModel> _expenseCategories { get; set; } = new();
    private List<BankModel> _banks { get; set; } = new();
    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }

    private string _dropdownFilterLabelExpense { get; set; } = Label.NoFilterAssigned;
    private string _dropdownFilterLabelBank { get; set; } = Label.NoFilterAssigned;

    public AdminExpenseFilterModal()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dropdownFilterLabelExpense = await _dropdownFilterService.UpdateLabel(Label.FilterByExpenseCategory);
        _dropdownFilterLabelBank = await _dropdownFilterService.UpdateLabel(Label.FilterByBank);
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
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

            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }


    public async Task OpenModalAsync()
    {
        try
        {
            _modalTarget = Guid.NewGuid();

            await Task.FromResult(_modal.Open(_modalTarget));
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _expenseCategories = await _expenseCategoryService.GetRecords();
            _banks = await _bankService.GetRecords();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
}

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterExpenseCategory()
    {
        _filterExpenseCategory = new();
        _filterExpenseDTO.ECategoryId = 0;
        _filterExpenseCategoryModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelExpense = await _dropdownFilterService.UpdateLabel(Label.FilterByExpenseCategory);
        _toastService.ShowToast("Filter for expense removed!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterExpenseDTO);
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterBank()
    {
        _filterBank = new();
        _filterExpenseDTO.BankId = 0;
        _filterBankModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelBank = await _dropdownFilterService.UpdateLabel(Label.FilterByBank);
        _toastService.ShowToast("Filter for bank removed!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterExpenseDTO);
        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterExpenseCategory(ulong id)
    {
        _filterExpenseDTO.ECategoryId = id;
        _filterExpenseCategory = _expenseCategories.First(i => i.Id == id);
        string? expenseName = _filterExpenseCategory.Description.Truncate((int)Truncate.ExpenseCategory);

        _filterExpenseCategoryModel = await _dropdownFilterService.SetModel(_filterExpenseCategory.Id, _filterExpenseCategory.Description);

        _dropdownFilterLabelExpense = await _dropdownFilterService.UpdateLabel(expenseName!);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterExpenseDTO);
        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterBank(ulong id)
    {
        _filterExpenseDTO.BankId = id;
        _filterBank = _banks.First(i => i.Id == id);
        string? bankName = _filterBank.Description.Truncate((int)Truncate.Bank);

        _filterBankModel = await _dropdownFilterService.SetModel(_filterBank.Id, _filterBank.Description);

        _dropdownFilterLabelBank = await _dropdownFilterService.UpdateLabel(bankName!);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterExpenseDTO);
        await Task.CompletedTask;
    }
}
