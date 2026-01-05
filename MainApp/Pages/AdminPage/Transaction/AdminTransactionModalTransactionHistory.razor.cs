using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionModalTransactionHistory : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IBankService<BankModel> _bankService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private IBankTransactionHistoryService<BankTransactionHistoryModel> _bankTransactionHistoryService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private InputFormAttributes _inputFormAttributes { get; set; } = new();
    private DateTimeRange _dateRange { get; set; } = new();
    private List<BankModel> _banks { get; set; } = new();
    private List<BankTransactionHistoryByDateGroupDTO> _bankTransactionHistoryListView { get; set; } = new();
    private BankTransactionHistoryModel _bankTransactionHistoryModel { get; set; } = new();
    private MultiFilterBankTransactionHistoryDTO _multiFilterBankTransactionHistoryDTO { get; set; } = new();
    private bool _isLoading { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    public AdminTransactionModalTransactionHistory()
    {
    }

    private async Task FetchDataAsync()
    {

        _isLoading = true;

        StateHasChanged();

        try
        {
            _banks = await _bankService.GetRecords();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        finally
        {
            _isLoading = false;

            StateHasChanged();
        }
    }

    private async Task SearchAsync()
    {

        _multiFilterBankTransactionHistoryDTO.DateTimeRange = null;
        _multiFilterBankTransactionHistoryDTO.BankId = _bankTransactionHistoryModel.BankId;
        _multiFilterBankTransactionHistoryDTO.LoadMore = LoadMore.BankTransactionHistory;

        _isProcessing = true;

        StateHasChanged();

        try
        {
            _bankTransactionHistoryListView = await _bankTransactionHistoryService.GetRecordsListView(_multiFilterBankTransactionHistoryDTO);
            await Task.Delay((int)Delay.DataLoading);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        finally
        {
            _isProcessing = false;

            StateHasChanged();
        }
    }

    public async Task OpenModalAsync()
    {
        try
        {
            _modalTarget = Guid.NewGuid();

            _inputFormAttributes.Control = new()
            {
                {
                    "class", $"form-control rounded{_appSettings.Form}"
                }
            };

            _inputFormAttributes.Select = new()
            {
                {
                    "class", $"form-select form-select-sm rounded{_appSettings.Form}"
                }
            };

            await _modal.Open(_modalTarget);

            await FetchDataAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
    }

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }
}
