using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Pages.SetupPage.TransactionCategory;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.TransactionCategory;

public partial class SetupTransactionCategoryPanelLeft : ComponentBase
{
    [Inject]
    private ITransactionCategoryService<TransactionCategoryModel> _transactionCategoryService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private ISpinnerService _spinnerService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private SetupTransactionCategoryOffCanvas _setupOffCanvas { get; set; } = new();
    private TransactionCategoryModel _transactionCategoryModel { get; set; } = new();

    private List<TransactionCategoryModel> _transactionCategories { get; set; } = new();
    private List<TransactionCategoryModel> _searchResults { get; set; } = new();
    private string _searchTerm { get; set; } = string.Empty;
    private bool _isSearching { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private bool _searchButtonEnabled { get; set; } = false;

    public SetupTransactionCategoryPanelLeft()
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
                _searchResults = await _transactionCategoryService.GetSearchResults(_searchTerm);
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
            _transactionCategories = await _transactionCategoryService.GetRecords();
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(TransactionCategoryModel transactionCategoryModel)
    {
        try
        {
            await _transactionCategoryService.UpdateRecordStatus(transactionCategoryModel);
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
        TransactionCategoryModel updatedModel = _setupOffCanvas.DataModel;
        TransactionCategoryModel model = _transactionCategories.FirstOrDefault(b => b.Id == updatedModel.Id)!;

        var index = _transactionCategories.IndexOf(model);

        if (index != -1)
        {
            if (updatedModel.IsArchived)
            {
                var archivedModel = _transactionCategories[index] = updatedModel;
                _transactionCategories.Remove(archivedModel);
            }
            else
            {
                _transactionCategories[index] = updatedModel;
            }
        }
        else
        {
            _transactionCategories.Add(updatedModel);
        }

        await InvokeAsync(StateHasChanged);
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(TransactionCategoryModel transactionCategoryModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(transactionCategoryModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(TransactionCategoryModel transactionCategoryModel)
    {
        try
        {
            await _setupOffCanvas.ViewRecordOffCanvasAsync(transactionCategoryModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
