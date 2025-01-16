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

    private async Task RemoveDropdownFilterBank()
    {
        _multiFilterTransactionDTO.FromBank = new();
        _banks = await _dropDownMultiSelectService.UncheckAll(_banks);
        
        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTransactionDTO);


        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterTransactionCategory()
    {
        _multiFilterTransactionDTO.TCategoryId = new();
        _transactionCategories = await _dropDownMultiSelectService.UncheckAll(_transactionCategories);
        
        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTransactionDTO);


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
}
