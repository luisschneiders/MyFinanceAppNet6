using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;

namespace MainApp.Pages.SetupPage;

public partial class SetupBank : ComponentBase
{
    [Inject]
    IBankService _bankService { get; set; } = default!;

    private List<BankModel> _banks { get; set; } = new();

    public SetupBank()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        await FetchDataAsync();

        // Please delete me!
        await CreateDummyRecord();

        await Task.CompletedTask;
    }

    private async Task SearchAsync()
    {
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        _banks = await _bankService.GetBanks();

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(BankModel bankModel)
    {
        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(BankModel bankModel)
    {
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(BankModel bankModel)
    {
        await Task.CompletedTask;
    }


    // Please delete me!
    private async Task CreateDummyRecord()
    {
        Random rnd = new();
        var amount = rnd.Next(88, 888);

        BankModel bankModel = new()
        {
            Account = $"Account {DateTime.Now}",
            Description = $"Description {DateTime.Now}",
            InitialBalance = amount,
            CurrentBalance = amount
        };

        await _bankService.CreateBank(bankModel);

        bankModel.Id = await _bankService.GetLastInsertedId();

        _banks.Add(bankModel);

        await Task.CompletedTask;
    }

}
