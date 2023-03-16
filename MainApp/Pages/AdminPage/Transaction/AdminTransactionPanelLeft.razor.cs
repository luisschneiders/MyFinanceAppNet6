using MainApp.Components.Dropdown;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Pages.AdminPage.Timesheet;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionPanelLeft : ComponentBase
{
    [Inject]
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    /*
     * Add OffCanvas component reference
     */
    private AdminTransactionOffCanvas _setupOffCanvas { get; set; } = new();

    /*
     * Add Modal component reference
     */
    private AdminTransactionModal _setupModal { get; set; } = new();
    private AdminTransactionModalInfo _setupModalInfo { get; set; } = new();

    private DateTimeRange _dateTimeRange { get; set; } = new();

    private List<TransactionByCategoryGroupDTO> _transactionsByGroup { get; set; } = new();

    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;
    private bool _isLoading { get; set; } = true;

    public AdminTransactionPanelLeft()
    {
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
            _transactionsByGroup = await _transactionService.GetRecordsByGroupAndDateRange(_dateTimeRange);
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

    private async Task ArchiveRecordAsync(TransactionListDTO model)
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

    private async Task InfoRecordAsync()
    {
        await _setupModalInfo.OpenModalAsync();
    }

    private async Task RefreshList()
    {
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
}
