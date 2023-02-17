using MainApp.Components.Toast;
using MainApp.Pages.AdminPage.Timesheet;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionPanelLeft : ComponentBase
{
    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    /*
     * Add OffCanvas component reference
     */
    private AdminTransactionOffCanvas _setupOffCanvas { get; set; } = new();

    private DateTimeRangeModel _dateTimeRangeModel { get; set; } = new();

    private List<TransactionModelListDTO> _transactionsDummy { get; set; } = new();

    public AdminTransactionPanelLeft()
    {
        TransactionModelListDTO transactionModelListDTO = new();
        transactionModelListDTO.Id = 1;
        transactionModelListDTO.BankDescription = "Bank 1";
        transactionModelListDTO.TDate = DateTime.Now;
        transactionModelListDTO.TCategoryTypeDescription = "Transfer";
        transactionModelListDTO.Action = "D";
        transactionModelListDTO.Label = "T";
        transactionModelListDTO.Amount = 12;
        _transactionsDummy.Add(transactionModelListDTO);

        transactionModelListDTO = new();
        transactionModelListDTO.Id = 2;
        transactionModelListDTO.BankDescription = "Bank 2";
        transactionModelListDTO.TDate = DateTime.Now;
        transactionModelListDTO.TCategoryTypeDescription = "Purchases";
        transactionModelListDTO.Action = "C";
        transactionModelListDTO.Label = "T";
        transactionModelListDTO.Amount = 1234;
        _transactionsDummy.Add(transactionModelListDTO);

        transactionModelListDTO = new();
        transactionModelListDTO.Id = 3;
        transactionModelListDTO.BankDescription = "Bank 3";
        transactionModelListDTO.TDate = DateTime.Now;
        transactionModelListDTO.TCategoryTypeDescription = "ABN";
        transactionModelListDTO.Action = "C";
        transactionModelListDTO.Label = "C";
        transactionModelListDTO.Amount = 8888;
        _transactionsDummy.Add(transactionModelListDTO);
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRangeModel = _dateTimeService.GetCurrentMonth();
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await _setupOffCanvas.AddRecordOffCanvasAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshList()
    {
        //await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task RefreshListFromDropdownDateRange()
    {
        //await FetchDataAsync();
        _toastService.ShowToast("Date range has changed!", Theme.Info);
        await Task.CompletedTask;
    }
}
