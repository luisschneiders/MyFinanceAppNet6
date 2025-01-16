using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripPanelLeft : ComponentBase
{
    [Inject]
    private ITripService<TripModel> _tripService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();
    private List<TripByVehicleGroupDTO> _tripsByGroup { get; set; } = new();
    private MultiFilterTripDTO _multiFilterTripDTO { get; set; } = new();
    private PayStatus[] _payStatuses { get; set; } = default!;
    private TripCategory[] _tripCategories { get; set; } = default!;
    private decimal _sumByDateRange { get; set; }

    /*
     * Add component reference
     */
    private AdminTripOffCanvas _setupOffCanvas { get; set; } = new();
    private AdminTripModal _setupModal { get; set; } = new();
    private AdminTripModalFilter _setupFilterModal { get; set; } = new();
    private AdminTripModalPrinter _setupPrinterModal { get; set; } = new();

    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;
    private bool _isLoading { get; set; } = true;

    public AdminTripPanelLeft()
    {
        _payStatuses = (PayStatus[])Enum.GetValues(typeof(PayStatus));
        _tripCategories = (TripCategory[])Enum.GetValues(typeof(TripCategory));
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);

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
                _isLoading = false;
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _multiFilterTripDTO.DateTimeRange = _dateTimeRange;
            _tripsByGroup = await _tripService.GetRecordsListView(_multiFilterTripDTO);
            _sumByDateRange = await _tripService.GetSumByDateRange();
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task PrintAsync()
    {
        try
        {
            PrintTripDTO printTripDTO = new ()
            {
                DateTimeRange = _dateTimeRange,
                TripsByGroup = _tripsByGroup,
                SumByDateRange = _sumByDateRange,
            };

            await _setupPrinterModal.OpenModalAsync(printTripDTO);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task ArchiveRecordAsync(TripListDTO model)
    {
        try
        {
            await _setupModal.OpenModalAsync(model.Id.ToString());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdatePayStatusAsync(TripListDTO tripListDTO, int payStatus)
    {
        try
        {
            TripModel tripModel = new()
            {
                Id = tripListDTO.Id,
                PayStatus = payStatus
            };

            await _tripService.UpdateRecordPayStatus(tripModel);
            await RefreshList();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateTripCategoryAsync(TripListDTO tripListDTO, ulong tripCategory)
    {
        try
        {
            TripModel tripModel = new()
            {
                Id = tripListDTO.Id,
                TCategoryId = tripCategory
            };

            await _tripService.UpdateRecordTripCategory(tripModel);
            await RefreshList();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshFilterList(MultiFilterTripDTO multiFilterTripDTO)
    {
        _multiFilterTripDTO = multiFilterTripDTO;
        _multiFilterTripDTO.IsFilterChanged = true;

        await FetchDataAsync();

        await Task.CompletedTask;
    }

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _dateTimeRange = dateTimeRange;
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);

        await RefreshList();

        await Task.CompletedTask;
    }

    private string UpdatePayStatusTitle(int id)
    {
        var title = _payStatuses[id];

        return title.ToString();
    }

    private string UpdateTripCategoryTitle(ulong id)
    {
        string? title;

        if (id == (int)TripCategory.NotSpecified)
        {
            title = Label.NotSpecified;
        }
        else
        {
            title = _enumHelper.GetDescription(_tripCategories[id]);
        }

        return title;
    }

    private Theme UpdatePayStatusButton(int id)
    {
        if (id == (int)PayStatus.Paid)
        {
            return Theme.Success;
        }

        return Theme.Light;
    }

    private Theme UpdateTripCategoryButton(ulong id)
    {
        if (id == (int)TripCategory.NotSpecified)
        {
            return Theme.Warning;
        }

        return Theme.Light;
    }

    private async Task ApplyFiltersAsync()
    {
        try
        {
            await _setupFilterModal.OpenModalAsync(IsFilterApplied());
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ResetAllFilters()
    {
        _multiFilterTripDTO = new();

        await FetchDataAsync();

        await Task.CompletedTask;
    }

    private bool IsFilterApplied()
    {
        if (_multiFilterTripDTO.VehicleId.Count > 0 || _multiFilterTripDTO.TCategoryId.Count > 0)
        {
            return true;
        }
        else{
            return false;
        }
    }
}
