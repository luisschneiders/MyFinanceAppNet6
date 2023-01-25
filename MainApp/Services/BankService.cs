using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.Sql;

namespace MainApp.Services;

public class BankService : IBankService
{
    [Inject]
    IBankData _bankData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public BankService(IBankData bankData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _bankData = bankData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task<List<BankModel>> GetBanks()
    {
        var user = await GetLoggedInUser();
        List<BankModel> results = await _bankData.GetBanks(user.Id);
        return results;
    }


    public async Task<BankModel> GetBankById(string bankId)
    {
        var user = await GetLoggedInUser();
        BankModel result = await _bankData.GetBankById(user.Id, bankId);
        return result;
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _bankData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task CreateBank(BankModel bankModel)
    {
        var user = await GetLoggedInUser();
        BankModel newBank = new()
        {
            Account = bankModel.Account,
            Description = bankModel.Description,
            InitialBalance = bankModel.InitialBalance,
            CurrentBalance = bankModel.InitialBalance,
            UpdatedBy = user.Id
        };

        await _bankData.CreateBank(newBank);
        await Task.CompletedTask;
    }

    public async Task UpdateBank(BankModel bankModel)
    {
        var user = await GetLoggedInUser();
        BankModel newBank = new()
        {
            Id = bankModel.Id,
            Account = bankModel.Account,
            Description = bankModel.Description,
            CurrentBalance = bankModel.CurrentBalance,
            IsActive = bankModel.IsActive,
            UpdatedBy = user.Id,
            UpdatedAt = DateTime.Now,
        };

        await _bankData.UpdateBank(newBank);
        await Task.CompletedTask;
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }

}
