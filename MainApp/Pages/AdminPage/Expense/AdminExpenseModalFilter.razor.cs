using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpenseModalFilter : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;

    [Inject]
    private IBankService<BankModel> _bankService { get; set; } = default!;

    [Inject]
    private IDropdownMultiSelectService _dropDownMultiSelectService { get;set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public EventCallback<MultiFilterExpenseDTO> OnSubmitFilterSuccess { get; set; }

    private MultiFilterExpenseDTO _multiFilterExpenseDTO { get; set; } = new();
    private List<CheckboxItemModel> _expenseCategories { get; set; } = new();
    private List<CheckboxItemModel> _banks { get; set; } = new();
    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private string _searchQueryExpense = string.Empty;
    private string _searchQueryInstitution = string.Empty;
    private bool _selectAllCheckedExpense = false;
    private bool _selectAllCheckedInstitution = false;
    private List<CheckboxItemModel> _filteredExpenseCategories => 
        string.IsNullOrWhiteSpace(_searchQueryExpense) 
            ? _expenseCategories 
            : _expenseCategories.Where(ec => ec.Description.Contains(_searchQueryExpense, StringComparison.OrdinalIgnoreCase)).ToList();
    private List<CheckboxItemModel> _filteredInstitutions => 
        string.IsNullOrWhiteSpace(_searchQueryInstitution) 
            ? _banks 
            : _banks.Where(ec => ec.Description.Contains(_searchQueryInstitution, StringComparison.OrdinalIgnoreCase)).ToList();

    public AdminExpenseModalFilter()
    {
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

    public async Task OpenModalAsync(bool isFilterApplied)
    {
        try
        {
            _modalTarget = Guid.NewGuid();

            if (isFilterApplied is false)
            {
                await ResetAllFilters();
            }

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
            _expenseCategories = await _expenseCategoryService.GetRecordsForFilter();
            _banks = await _bankService.GetRecordsForFilter();
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

    private async Task ResetAllFilters()
    {
        await UncheckAll();
        await RemoveAllFilters();

        await Task.CompletedTask;
    }

    private async Task UncheckAll()
    {
        _banks = await _dropDownMultiSelectService.UncheckAll(_banks);
        _expenseCategories = await _dropDownMultiSelectService.UncheckAll(_expenseCategories);

        await Task.CompletedTask;
    }

    private async Task RemoveAllFilters()
    {
        _multiFilterExpenseDTO.BankId = new();
        _multiFilterExpenseDTO.ECategoryId = new();

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterExpenseDTO);

        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedExpense(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel expense = _expenseCategories.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterExpenseDTO.ECategoryId.Add(id);
            expense.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterExpenseDTO.ECategoryId.Remove(id);
            expense.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterExpenseDTO);
        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedBank(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel bank = _banks.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterExpenseDTO.BankId.Add(id);
            bank.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterExpenseDTO.BankId.Remove(id);
            bank.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterExpenseDTO);
        await Task.CompletedTask;
    }

    private async void ToggleSelectAllExpense(ChangeEventArgs e)
    {
        _selectAllCheckedExpense = (bool)e.Value!;
        foreach (var expenseCategory in _expenseCategories)
        {
            expenseCategory.IsChecked = _selectAllCheckedExpense;

            if (e.Value is true)
            {
                _multiFilterExpenseDTO.ECategoryId.Add(expenseCategory.Id);
                expenseCategory.IsChecked = true;
            }
            else if (e.Value is false)
            {
                _multiFilterExpenseDTO.ECategoryId.Remove(expenseCategory.Id);
                expenseCategory.IsChecked = false;
            }

        }
        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterExpenseDTO);
    }
    private async void ToggleSelectAllInstitution(ChangeEventArgs e)
    {
        _selectAllCheckedInstitution = (bool)e.Value!;
        foreach (var bank in _banks)
        {
            bank.IsChecked = _selectAllCheckedInstitution;

            if (e.Value is true)
            {
                _multiFilterExpenseDTO.ECategoryId.Add(bank.Id);
                bank.IsChecked = true;
            }
            else if (e.Value is false)
            {
                _multiFilterExpenseDTO.ECategoryId.Remove(bank.Id);
                bank.IsChecked = false;
            }

        }
        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterExpenseDTO);
    }
}
