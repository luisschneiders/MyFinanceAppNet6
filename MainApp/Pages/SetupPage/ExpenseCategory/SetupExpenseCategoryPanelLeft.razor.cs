using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Pages.SetupPage.ExpenseCategory;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.ExpenseCategory;

public partial class SetupExpenseCategoryPanelLeft : ComponentBase
{
    [Inject]
    private IExpenseCategoryService<ExpenseCategoryModel> _expenseCategoryService { get; set; } = default!;

    [Inject]
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private ISpinnerService _spinnerService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private SetupExpenseCategoryOffCanvas _setupOffCanvas { get; set; } = new();
    private ExpenseCategoryModel _expenseCategoryModel { get; set; } = new();

    private List<ExpenseCategoryModel> _expenseCategories { get; set; } = new();
    private List<ExpenseAmountHistoryDTO> _expenseAmountHistory { get; set; } = new();
    private List<ExpenseCategoryModel> _searchResults { get; set; } = new();
    private string _searchTerm { get; set; } = string.Empty;
    private bool _isSearching { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private bool _searchButtonEnabled { get; set; } = false;

    public SetupExpenseCategoryPanelLeft()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _spinnerService.ShowSpinner();
        }

        await Task.CompletedTask;
    }

    private async Task SearchTermAsync(ChangeEventArgs eventArgs)
    {
        var searchTerm = eventArgs?.Value?.ToString();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            _searchResults = new();
            _isSearching = false;
            _searchButtonEnabled = false;
        }
        else
        {
            _searchButtonEnabled = true;
        }

        await Task.CompletedTask;
    }

    private async Task SearchAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_searchTerm))
            {
                _isSearching = true;
                _searchResults = await _expenseCategoryService.GetSearchResults(_searchTerm);
                StateHasChanged();
            }
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
            _expenseAmountHistory = await _expenseService.GetAmountHistory();
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(ExpenseCategoryModel expenseCategoryModel)
    {
        try
        {
            await _expenseCategoryService.UpdateRecordStatus(expenseCategoryModel);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        // TODO: add service to refresh the list
        ExpenseCategoryModel updatedModel = _setupOffCanvas.DataModel;
        ExpenseCategoryModel model = _expenseCategories.FirstOrDefault(b => b.Id == updatedModel.Id)!;

        var index = _expenseCategories.IndexOf(model);

        if (index != -1)
        {
            if (updatedModel.IsArchived)
            {
                var archivedModel = _expenseCategories[index] = updatedModel;
                _expenseCategories.Remove(archivedModel);
            }
            else
            {
                _expenseCategories[index] = updatedModel;
            }
        }
        else
        {
            _expenseCategories.Add(updatedModel);
        }

        await InvokeAsync(StateHasChanged);
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(ExpenseCategoryModel expenseCategoryModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(expenseCategoryModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(ExpenseCategoryModel expenseCategoryModel)
    {
        try
        {
            await _setupOffCanvas.ViewRecordOffCanvasAsync(expenseCategoryModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
