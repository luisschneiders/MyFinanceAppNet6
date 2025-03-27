using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionModalFilter : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDropdownMultiSelectService _dropDownMultiSelectService { get; set; } = default!;

    [Inject]
    private ITransactionCategoryService<TransactionCategoryModel> _transactionCategoryService { get; set; } = default!;

    [Inject]
    private IBankService<BankModel> _bankService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public EventCallback<MultiFilterTransactionDTO> OnSubmitFilterSuccess { get; set; }

    private MultiFilterTransactionDTO _multiFilterTransactionDTO { get; set; } = new();
    private List<CheckboxItemModel> _transactionCategories { get; set; } = new();
    private List<CheckboxItemModel> _banks { get; set; } = new();
    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private string _searchQueryTransaction = string.Empty;
    private string _searchQueryInstitution = string.Empty;
    private bool _selectAllCheckedTransaction = false;
    private bool _selectAllCheckedInstitution = false;
    private List<CheckboxItemModel> _filteredTransactionCategories => 
        string.IsNullOrWhiteSpace(_searchQueryTransaction) 
            ? _transactionCategories 
            : _transactionCategories.Where(ec => ec.Description.Contains(_searchQueryTransaction, StringComparison.OrdinalIgnoreCase)).ToList();
    private List<CheckboxItemModel> _filteredInstitutions => 
        string.IsNullOrWhiteSpace(_searchQueryInstitution) 
            ? _banks 
            : _banks.Where(ec => ec.Description.Contains(_searchQueryInstitution, StringComparison.OrdinalIgnoreCase)).ToList();

    public AdminTransactionModalFilter()
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
            _transactionCategories = await _transactionCategoryService.GetRecordsForFilter();
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
        _transactionCategories = await _dropDownMultiSelectService.UncheckAll(_transactionCategories);

        await Task.CompletedTask;
    }

    private async Task RemoveAllFilters()
    {
        _multiFilterTransactionDTO.FromBank = new();
        _multiFilterTransactionDTO.TCategoryId = new();
        
        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTransactionDTO);
        
        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedTransaction(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel transaction = _transactionCategories.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterTransactionDTO.TCategoryId.Add(id);
            transaction.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterTransactionDTO.TCategoryId.Remove(id);
            transaction.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTransactionDTO);
        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedBank(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel bank = _banks.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterTransactionDTO.FromBank.Add(id);
            bank.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterTransactionDTO.FromBank.Remove(id);
            bank.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTransactionDTO);
        await Task.CompletedTask;
    }

    private async void ToggleSelectAllTransaction(ChangeEventArgs e)
    {
        _selectAllCheckedTransaction = (bool)e.Value!;

        foreach (var transactionCategory in _transactionCategories)
        {
            transactionCategory.IsChecked = _selectAllCheckedTransaction;

            if (e.Value is true)
            {
                _multiFilterTransactionDTO.TCategoryId.Add(transactionCategory.Id);
                transactionCategory.IsChecked = true;
            }
            else if (e.Value is false)
            {
                _multiFilterTransactionDTO.TCategoryId.Remove(transactionCategory.Id);
                transactionCategory.IsChecked = false;
            }
        }

        if (_selectAllCheckedTransaction is false)
        {
            _multiFilterTransactionDTO.TCategoryId = new();
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTransactionDTO);
    }

    private async void ToggleSelectAllInstitution(ChangeEventArgs e)
    {
        _selectAllCheckedInstitution = (bool)e.Value!;

        foreach (var bank in _banks)
        {
            bank.IsChecked = _selectAllCheckedInstitution;

            if (e.Value is true)
            {
                _multiFilterTransactionDTO.FromBank.Add(bank.Id);
                bank.IsChecked = true;
            }
            else if (e.Value is false)
            {
                _multiFilterTransactionDTO.FromBank.Remove(bank.Id);
                bank.IsChecked = false;
            }
        }

        if (_selectAllCheckedInstitution is false)
        {
            _multiFilterTransactionDTO.FromBank = new();
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTransactionDTO);
    }
}
