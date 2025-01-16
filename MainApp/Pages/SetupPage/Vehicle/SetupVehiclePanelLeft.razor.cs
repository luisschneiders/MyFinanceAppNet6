using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.Vehicle;

public partial class SetupVehiclePanelLeft : ComponentBase
{
    [Inject]
    IVehicleService<VehicleModel> _vehicleService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    /*
     * Add OffCanvas component reference
     */
    private SetupVehicleOffCanvas _setupOffCanvas { get; set; } = new();
    private VehicleModel _vehicleModel { get; set; } = new();

    private List<VehicleModel> _vehicles { get; set; } = new();
    private List<VehicleModel> _searchResults { get; set; } = new();
    private string _searchTerm { get; set; } = string.Empty;
    private bool _isSearching { get; set; } = false;
    private bool _isLoading { get; set; } = true;
    private bool _searchButtonEnabled { get; set; } = false;

    public SetupVehiclePanelLeft()
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
                _searchResults = await _vehicleService.GetSearchResults(_searchTerm);
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
            _vehicles = await _vehicleService.GetRecords();
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(VehicleModel vehicleModel)
    {
        try
        {
            await _vehicleService.UpdateRecordStatus(vehicleModel);
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
        VehicleModel updatedModel = _setupOffCanvas.DataModel;
        VehicleModel model = _vehicles.FirstOrDefault(b => b.Id == updatedModel.Id)!;

        var index = _vehicles.IndexOf(model);

        if (index != -1)
        {
            if (updatedModel.IsArchived)
            {
                var archivedModel = _vehicles[index] = updatedModel;
                _vehicles.Remove(archivedModel);
            }
            else
            {
                _vehicles[index] = updatedModel;
            }
        }
        else
        {
            _vehicles.Add(updatedModel);
        }

        await InvokeAsync(StateHasChanged);
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(VehicleModel vehicleModel)
    {
        try
        {
            await _setupOffCanvas.EditRecordOffCanvasAsync(vehicleModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(VehicleModel vehicleModel)
    {
        try
        {
            await _setupOffCanvas.ViewRecordOffCanvasAsync(vehicleModel.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
