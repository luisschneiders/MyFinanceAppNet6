using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Pages.SetupPage.Expense;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.Expense;

public partial class SetupExpenseLeftPanel : ComponentBase
{
    [Inject]
    IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private SetupExpenseOffCanvas _setupOffCanvas { get; set; } = new();
    private ExpenseModel _expenseModel { get; set; } = new();

    private List<ExpenseModel> _expenses { get; set; } = new();
    private List<ExpenseModel> _searchResults { get; set; } = new();
    private string _searchTerm { get; set; } = string.Empty;
    private bool _isSearching { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private bool _searchButtonEnabled { get; set; } = false;

    public SetupExpenseLeftPanel()
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
            await Task.Run(() => _spinnerService.ShowSpinner());
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
                _searchResults = await _expenseService.GetSearchResults(_searchTerm);
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);

        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _expenses = await _expenseService.GetRecords();
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(ExpenseModel expenseModel)
    {
        try
        {
            await _expenseService.UpdateRecordStatus(expenseModel);
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        // TODO: add service to refresh the list
        ExpenseModel updatedModel = _setupOffCanvas.DataModel;
        ExpenseModel model = _expenses.FirstOrDefault(b => b.Id == updatedModel.Id)!;

        var index = _expenses.IndexOf(model);

        if (index != -1)
        {
            if (updatedModel.IsArchived)
            {
                var archivedModel = _expenses[index] = updatedModel;
                _expenses.Remove(archivedModel);
            }
            else
            {
                _expenses[index] = updatedModel;
            }
        }
        else
        {
            _expenses.Add(updatedModel);
        }

        await InvokeAsync(StateHasChanged);
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(ExpenseModel expenseModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(expenseModel.Id.ToString());
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(ExpenseModel expenseModel)
    {
        try
        {
            await _setupOffCanvas.ViewRecordOffCanvasAsync(expenseModel.Id.ToString());
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }
}
