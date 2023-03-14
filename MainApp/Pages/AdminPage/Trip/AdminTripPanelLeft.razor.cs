using MainApp.Components.Dropdown;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Pages.AdminPage.Expense;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripPanelLeft : ComponentBase
{
    [Inject]
    private ITripService<TripModel> _tripService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();

    private List<TripListDTO> _trips { get; set; } = new();
    private decimal _sumByDateRange { get; set; }

    /*
     * Add OffCanvas component reference
     */
    private AdminTripOffCanvas _setupOffCanvas { get; set; } = new();

    /*
     * Add Modal component reference
     */
    private AdminTripModal _setupModal { get; set; } = new();

    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;
    private bool _isDateTimeRangeChanged { get; set; } = false;
    private bool _isLoading { get; set; } = true;

    public AdminTripPanelLeft()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        _dropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(_dateTimeRange);
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();
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
            _trips = await _tripService.GetRecordsByDateRange(_dateTimeRange);
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

    private async Task RefreshList()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshListFromDropdownDateRange()
    {
        _dropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(_dateTimeRange);
        _isDateTimeRangeChanged = true;
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await RefreshList();
        await Task.CompletedTask;
    }

    private async Task ResetDateTimeRange()
    {
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        _dropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(_dateTimeRange);
        _isDateTimeRangeChanged = false;
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await RefreshList();
        await Task.CompletedTask;
    }
}
