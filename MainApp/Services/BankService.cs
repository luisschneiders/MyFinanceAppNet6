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
        try
        {
            var user = await GetLoggedInUser();
            List<BankModel> results = await _bankData.GetBanks(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<BankModel> GetBankById(string bankId)
    {
        try
        {
            var user = await GetLoggedInUser();
            BankModel result = await _bankData.GetBankById(user.Id, bankId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<BankModel>> GetSearchResults(string search)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<BankModel> results = await _bankData.GetSearchResults(user.Id, search);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _bankData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task CreateBank(BankModel bankModel)
    {
        try
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
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateBank(BankModel bankModel)
    {
        try
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
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateBankStatus(BankModel bankModel)
    {
        try
        {
            var user = await GetLoggedInUser();
            BankModel newBank = new()
            {
                Id = bankModel.Id,
                IsActive = !bankModel.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _bankData.UpdateBankStatus(newBank);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }
}
