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

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private InputFormAttributes _inputFormAttributes { get; set; } = new();
    private List<BankModel> _activeBanks { get; set; } = new();
    private BankTransactionHistoryModel _bankTransactionHistoryModel { get; set; } = new();
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
            _activeBanks = await _bankService.GetRecordsActive();
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
        Console.WriteLine($"LFS - bank id {_bankTransactionHistoryModel.BankId}");
        _isProcessing = true;

        await Task.Delay((int)Delay.DataSuccess);

        _isProcessing = false;

        await Task.CompletedTask;
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
