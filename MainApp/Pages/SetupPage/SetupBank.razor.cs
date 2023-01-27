using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;

namespace MainApp.Pages.SetupPage;

public partial class SetupBank : ComponentBase
{
    [Inject]
    IBankService _bankService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private List<BankModel> _banks { get; set; } = new();
    private List<BankModel> _searchResults { get; set; } = new();
    private string _searchTerm { get; set; } = string.Empty;
    private bool _isSearching { get; set; } = false;
    private bool _searchButtonEnabled { get; set; } = false;

    public SetupBank()
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
                _searchResults = await _bankService.GetSearchResults(_searchTerm);
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);

        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _banks = await _bankService.GetBanks();
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(BankModel bankModel)
    {
        try
        {
            await _bankService.UpdateBankStatus(bankModel);
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(BankModel bankModel)
    {
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        // PLEASE DELETE ME!!!!
        await CreateDummyRecord();

        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(BankModel bankModel)
    {
        // PLEASE DELETE ME!!!!
        await UpdateDummyRecord(bankModel);

        await Task.CompletedTask;
    }

    // PLEASE DELETE ME!!!!
    private async Task CreateDummyRecord()
    {
        try
        {
            Random rnd = new();
            var amount = rnd.Next(88, 888);

            BankModel bankModel = new()
            {
                Account = $"Account added {DateTime.Now}",
                Description = $"Description aded {DateTime.Now}",
                InitialBalance = amount,
                CurrentBalance = amount
            };

            await _bankService.CreateBank(bankModel);

            bankModel.Id = await _bankService.GetLastInsertedId();

            _banks.Add(bankModel);
        }
        catch (Exception ex)
        {
            await Task.Delay((int)Delay.DataError);
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
    }

    // PLEASE DELETE ME!!!!
    private async Task UpdateDummyRecord(BankModel bankModel)
    {
        Random rnd = new();
        var amount = rnd.Next(88, 8888);

        bankModel.Account = $"Account updated {DateTime.Now}";
        bankModel.Description = $"Description updated {DateTime.Now}";
        bankModel.CurrentBalance = amount;
        bankModel.IsActive = false;

        await _bankService.UpdateBank(bankModel);

        await Task.CompletedTask;
    }
}
