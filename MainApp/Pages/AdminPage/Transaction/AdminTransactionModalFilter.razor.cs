using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionModalFilter : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDropdownFilterService _dropdownFilterService { get; set; } = default!;

    [Inject]
    private ITransactionCategoryService<TransactionCategoryModel> _transactionCategoryService { get; set; } = default!;

    [Inject]
    private IBankService<BankModel> _bankService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback<FilterTransactionDTO> OnSubmitFilterSuccess { get; set; }

    private Modal _modal { get; set; } = new();
    private FilterModel _filterTransactionCategoryModel { get; set; } = new();
    private FilterModel _filterBankModel { get; set; } = new();
    private TransactionCategoryModel _filterTransactionCategory { get; set; } = new();
    private List<TransactionCategoryModel> _transactionCategories { get; set; } = new();
    private List<BankModel> _banks { get; set; } = new();
    private BankModel _filterBank { get; set; } = new();
    private FilterTransactionDTO _filterTransactionDTO { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private string _dropdownFilterLabelTransaction { get; set; } = Label.NoFilterAssigned;
    private string _dropdownFilterLabelBank { get; set; } = Label.NoFilterAssigned;

    public AdminTransactionModalFilter()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dropdownFilterLabelTransaction = await _dropdownFilterService.UpdateLabel(Label.FilterByTransactionCategory);
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

    private async Task FetchDataAsync()
    {
        try
        {
            _transactionCategories = await _transactionCategoryService.GetRecords();
            _banks = await _bankService.GetRecords();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
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

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterTransactionCategory()
    {
        await RemoveDropdownFilterTransactionCategory();
        
        _toastService.ShowToast("Filter for transaction removed!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTransactionDTO);
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterBank()
    {
        await RemoveDropdownFilterBank();
        
        _toastService.ShowToast("Filter for bank removed!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTransactionDTO);
        await Task.CompletedTask;
    }

    private async Task ResetAllFilters()
    {
        await RemoveDropdownFilterBank();
        await RemoveDropdownFilterTransactionCategory();
        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterTransactionCategory()
    {
        _filterTransactionCategory = new();
        _filterTransactionDTO.TCategoryId = 0;
        _filterTransactionCategoryModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelTransaction = await _dropdownFilterService.UpdateLabel(Label.FilterByTransactionCategory);

        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterBank()
    {
        _filterBank = new();
        _filterTransactionDTO.FromBank = 0;
        _filterBankModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelBank = await _dropdownFilterService.UpdateLabel(Label.FilterByBank);

        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterTransactionCategory(ulong id)
    {
        _filterTransactionDTO.TCategoryId = id;
        _filterTransactionCategory = _transactionCategories.First(i => i.Id == id);
        string? transactionName = _filterTransactionCategory.Description.Truncate((int)Truncate.TransactionCategory);

        _filterTransactionCategoryModel = await _dropdownFilterService.SetModel(_filterTransactionCategory.Id, _filterTransactionCategory.Description);

        _dropdownFilterLabelTransaction = await _dropdownFilterService.UpdateLabel(transactionName!);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTransactionDTO);
        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterBank(ulong id)
    {
        _filterTransactionDTO.FromBank = id;
        _filterBank = _banks.First(i => i.Id == id);
        string? bankName = _filterBank.Description.Truncate((int)Truncate.Bank);

        _filterBankModel = await _dropdownFilterService.SetModel(_filterBank.Id, _filterBank.Description);

        _dropdownFilterLabelBank = await _dropdownFilterService.UpdateLabel(bankName!);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTransactionDTO);
        await Task.CompletedTask;
    }

}
