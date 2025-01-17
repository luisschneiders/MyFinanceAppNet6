using MainApp.Components.Modal;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionModalDetails : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public EventCallback<DateTime> OnSubmitSuccess { get; set; }

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private DateTimeRange _dateTimeRange { get; set; } = new();
    private List<TransactionDetailsDTO> _transactionList {get; set; } = new();
    private bool _isLoading { get; set; } = true;

    public AdminTransactionModalDetails()
    {
    }

    public async Task OpenModalAsync(DateTime date)
    {
        try
        {
            _modalTarget = Guid.NewGuid();
            _dateTimeRange.Start = date;
            _dateTimeRange.End = date;

            await _modal.Open(_modalTarget);

            await FetchDataAsync();

            await InvokeAsync(StateHasChanged);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task CloseModalAsync()
    {
        await ResetAsync();
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await CloseModalAsync();
        await OnSubmitSuccess.InvokeAsync(_dateTimeRange.Start);
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _transactionList = await _transactionService.GetRecordsDateView(_dateTimeRange);
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ResetAsync()
    {
        _transactionList = new();
        _isLoading = true;

        await Task.CompletedTask;
    }
}
