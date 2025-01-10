using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.TaxCategory;

public partial class SetupTaxCategoryPanelLeft : ComponentBase
{
    [Inject]
    private ITaxCategoryService<TaxCategoryModel> _taxCategoryService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private SetupTaxCategoryOffCanvas _setupOffCanvas { get; set; } = new();
    private TaxCategoryModel _taxCategoryModel { get; set; } = new();

    private List<TaxCategoryModel> _taxCategories { get; set; } = new();
    private List<TaxCategoryModel> _searchResults { get; set; } = new();
    private string _searchTerm { get; set; } = string.Empty;
    private bool _isSearching { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private bool _searchButtonEnabled { get; set; } = false;

    public SetupTaxCategoryPanelLeft()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        await FetchDataAsync();
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
                _searchResults = await _taxCategoryService.GetSearchResults(_searchTerm);
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
            _taxCategories = await _taxCategoryService.GetRecords();
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(TaxCategoryModel taxCategoryModel)
    {
        try
        {
            await _taxCategoryService.UpdateRecordStatus(taxCategoryModel);
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
        TaxCategoryModel updatedModel = _setupOffCanvas.DataModel;
        TaxCategoryModel model = _taxCategories.FirstOrDefault(b => b.Id == updatedModel.Id)!;

        var index = _taxCategories.IndexOf(model);

        if (index != -1)
        {
            if (updatedModel.IsArchived)
            {
                var archivedModel = _taxCategories[index] = updatedModel;
                _taxCategories.Remove(archivedModel);
            }
            else
            {
                _taxCategories[index] = updatedModel;
            }
        }
        else
        {
            _taxCategories.Add(updatedModel);
        }

        await InvokeAsync(StateHasChanged);
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(TaxCategoryModel taxCategoryModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(taxCategoryModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(TaxCategoryModel taxCategoryModel)
    {
        try
        {
            await _setupOffCanvas.ViewRecordOffCanvasAsync(taxCategoryModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
