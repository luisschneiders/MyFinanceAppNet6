using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.Sql;

namespace MainApp.Services;

public class BankService : IBankService<BankModel>
{
    [Inject]
    private IBankData<BankModel> _bankData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public BankService(IBankData<BankModel> bankData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _bankData = bankData;
        _userData = userData;
        _authProvider = authProvider;
    }

    // TODO: Add pagination capabilities
    public async Task<List<BankModel>> GetRecords()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<BankModel> results = await _bankData.GetRecords(user.Id);
            return results;
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

    public async Task<BankModel> GetRecordById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            BankModel result = await _bankData.GetRecordById(user.Id, modelId);
            return result;
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

    public async Task CreateRecord(BankModel model)
    {
        try
        {
            var user = await GetLoggedInUser();
            BankModel recordModel = new()
            {
                Account = model.Account,
                Description = model.Description,
                InitialBalance = model.CurrentBalance,
                CurrentBalance = model.CurrentBalance,
                UpdatedBy = user.Id
            };

            await _bankData.CreateRecord(recordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecord(BankModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();
            BankModel recordModel = new()
            {
                Id = model.Id,
                Account = model.Account,
                Description = model.Description,
                CurrentBalance = model.CurrentBalance,
                IsActive = model.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _bankData.UpdateRecord(recordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(BankModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();

            BankModel recordModel = model;
            recordModel.IsActive = !model.IsActive;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _bankData.UpdateRecordStatus(recordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveRecord(BankModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            BankModel recordModel = model;
            recordModel.IsArchived = true;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _bankData.ArchiveRecord(recordModel);
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
