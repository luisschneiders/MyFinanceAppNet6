using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Pages.SetupPage.Company;
using Microsoft.AspNetCore.Components;
using MongoDB.Driver;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage.Company;

public partial class SetupCompanyPanelLeft : ComponentBase
{
    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private SetupCompanyOffCanvas _setupOffCanvas { get; set; } = new();
    private CompanyModel _companyModel { get; set; } = new();

    private List<CompanyModel> _companies { get; set; } = new();
    private List<CompanyModel> _searchResults { get; set; } = new();
    private string _searchTerm { get; set; } = string.Empty;
    private bool _isSearching { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private bool _searchButtonEnabled { get; set; } = false;

    public SetupCompanyPanelLeft()
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
                _searchResults = await _companyService.GetSearchResults(_searchTerm);
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
            _companies = await _companyService.GetRecords();
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

    private async Task UpdateStatusAsync(CompanyModel companyModel)
    {
        try
        {
            await _companyService.UpdateRecordStatus(companyModel);
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
        CompanyModel updatedModel = _setupOffCanvas.DataModel;
        CompanyModel model = _companies.FirstOrDefault(b => b.Id == updatedModel.Id)!;

        var index = _companies.IndexOf(model);

        if (index != -1)
        {
            if (updatedModel.IsArchived)
            {
                var archivedModel = _companies[index] = updatedModel;
                _companies.Remove(archivedModel);
            }
            else
            {
                _companies[index] = updatedModel;
            }
        }
        else
        {
            _companies.Add(updatedModel);
        }

        await InvokeAsync(StateHasChanged);
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(CompanyModel companyModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(companyModel.Id.ToString());
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(CompanyModel companyModel)
    {
        try
        {
            await _setupOffCanvas.ViewRecordOffCanvasAsync(companyModel.Id.ToString());
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }
}
