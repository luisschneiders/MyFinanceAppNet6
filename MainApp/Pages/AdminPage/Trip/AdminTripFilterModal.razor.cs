using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripFilterModal : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDropdownFilterService _dropdownFilterService { get; set; } = default!;

    [Inject]
    private IVehicleService<VehicleModel> _vehicleService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback<FilterTripDTO> OnSubmitFilterSuccess { get; set; }

    private List<VehicleModel> _vehicles { get; set; } = new();
    private List<TripCategoryDTO> _tripCategoryDTOs { get; set; } = new();
    private TripCategoryDTO _tripCategory { get; set; } = new();
    private FilterModel _filterVehicleModel { get; set; } = new();
    private FilterModel _filterTripCategoryModel { get; set; } = new();
    private FilterTripDTO _filterTripDTO { get; set; } = new();
    private TripCategoryDTO _filterTripCategory { get; set; } = new();
    private VehicleModel _filterVehicle { get; set; } = new();
    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private TripCategory[] _tripCategories { get; set; } = default!;
    private string _dropdownFilterLabelVehicle { get; set; } = Label.NoFilterAssigned;
    private string _dropdownFilterLabelTripCategory { get; set; } = Label.NoFilterAssigned;

    public AdminTripFilterModal()
    {
        _tripCategories = (TripCategory[])Enum.GetValues(typeof(TripCategory));
    }

    protected async override Task OnInitializedAsync()
    {
        _dropdownFilterLabelVehicle = await _dropdownFilterService.UpdateLabel(Label.FilterByVehicle);
        _dropdownFilterLabelTripCategory = await _dropdownFilterService.UpdateLabel(Label.FilterByTripCategory);

        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                foreach (var (item, index) in _tripCategories.Select((value, index) => (value, index)))
                {
                    _tripCategory = new() {
                        Id = (ulong)index,
                        Description = _enumHelper.GetDescription(item),
                    };
                    _tripCategoryDTOs.Add(_tripCategory);
                }

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

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _vehicles = await _vehicleService.GetRecords();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterVehicle()
    {
        await RemoveDropdownFilterVehicle();
        
        _toastService.ShowToast("Filter for vehicle removed!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTripDTO);
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterTripCategory()
    {
        await RemoveDropdownFilterTripCategory();
        
        _toastService.ShowToast("Filter for category removed!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTripDTO);
        await Task.CompletedTask;
    }

    private async Task ResetAllFilters()
    {
        await RemoveDropdownFilterVehicle();
        await RemoveDropdownFilterTripCategory();
        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterVehicle()
    {
        _filterVehicle = new();
        _filterTripDTO.VehicleId = 0;
        _filterVehicleModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelVehicle = await _dropdownFilterService.UpdateLabel(Label.FilterByVehicle);

        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterTripCategory()
    {
        _filterTripCategory = new();
        _filterTripDTO.TCategoryId = 0;
        _filterTripCategoryModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelTripCategory = await _dropdownFilterService.UpdateLabel(Label.FilterByTripCategory);

        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterVehicle(ulong id)
    {
        _filterTripDTO.VehicleId = id;
        _filterVehicle = _vehicles.First(i => i.Id == id);
        _filterVehicleModel = await _dropdownFilterService.SetModel(_filterVehicle.Id, _filterVehicle.Description);
        _dropdownFilterLabelVehicle = await _dropdownFilterService.UpdateLabel(_filterVehicle.Description);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTripDTO);
        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterTripCategory(ulong id)
    {
        _filterTripDTO.TCategoryId = id;
        _filterTripCategory = _tripCategoryDTOs.First(i => i.Id == id);
        _filterTripCategoryModel = await _dropdownFilterService.SetModel(_filterTripCategory.Id, _filterTripCategory.Description);
        _dropdownFilterLabelTripCategory = await _dropdownFilterService.UpdateLabel(_filterTripCategory.Description);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTripDTO);
        await Task.CompletedTask;
    }

}
