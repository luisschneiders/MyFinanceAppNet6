using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripModalFilter : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDropdownMultiSelectService _dropDownMultiSelectService { get; set; } = default!;

    [Inject]
    private IVehicleService<VehicleModel> _vehicleService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public EventCallback<MultiFilterTripDTO> OnSubmitFilterSuccess { get; set; }

    private List<CheckboxItemModel> _vehicles { get; set; } = new();
    private List<CheckboxItemModel> _tripCategoryDTOs { get; set; } = new();
    private CheckboxItemModel _tripCategory { get; set; } = new();
    private MultiFilterTripDTO _multiFilterTripDTO { get; set; } = new();
    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private TripCategory[] _tripCategories { get; set; } = default!;

    public AdminTripModalFilter()
    {
        _tripCategories = (TripCategory[])Enum.GetValues(typeof(TripCategory));
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

    private async Task FetchDataAsync()
    {
        try
        {
            _vehicles = await _vehicleService.GetRecordsForFilter();
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

    private async Task RemoveDropdownFilterVehicle()
    {
        _multiFilterTripDTO.VehicleId = new();
        _vehicles = await _dropDownMultiSelectService.UncheckAll(_vehicles);
        
        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTripDTO);

        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterTripCategory()
    {
        _multiFilterTripDTO.TCategoryId = new();
        _tripCategoryDTOs = await _dropDownMultiSelectService.UncheckAll(_tripCategoryDTOs);

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTripDTO);

        await Task.CompletedTask;
    }

    private async Task UncheckAll()
    {
        _vehicles = await _dropDownMultiSelectService.UncheckAll(_vehicles);
        _tripCategoryDTOs = await _dropDownMultiSelectService.UncheckAll(_tripCategoryDTOs);

        await Task.CompletedTask;
    }

    private async Task RemoveAllFilters()
    {
        _multiFilterTripDTO.VehicleId = new();
        _multiFilterTripDTO.TCategoryId = new();
        
        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTripDTO);
        
        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedVehicle(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel vehicle = _vehicles.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterTripDTO.VehicleId.Add(id);
            vehicle.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterTripDTO.VehicleId.Remove(id);
            vehicle.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTripDTO);
        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedTripCategory(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel tripCategory = _tripCategoryDTOs.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterTripDTO.TCategoryId.Add(id);
            tripCategory.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterTripDTO.TCategoryId.Remove(id);
            tripCategory.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTripDTO);
        await Task.CompletedTask;
    }
}
