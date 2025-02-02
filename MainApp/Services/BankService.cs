using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class BankService : IBankService<BankModel>
{
    [Inject]
    private IBankData<BankModel> _bankData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

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
            UserModel user = await GetLoggedInUser();
            List<BankModel> results = await _bankData.GetRecords(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<CheckboxItemModel>> GetRecordsForFilter()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<BankModel> results = await _bankData.GetRecords(user.Id);

            List<CheckboxItemModel> filter = new();

            foreach (var item in results)
            {
                CheckboxItemModel filterItem = new()
                {
                    Id = item.Id,
                    Description = item.Description,
                };
                filter.Add(filterItem);
            }

            return filter;
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
            UserModel user = await GetLoggedInUser();
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
            UserModel user = await GetLoggedInUser();
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
            UserModel user = await GetLoggedInUser();

            model.InitialBalance = model.CurrentBalance;
            model.UpdatedBy = user.Id;

            await _bankData.CreateRecord(model);
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
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _bankData.UpdateRecord(model);
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
            UserModel user = await GetLoggedInUser();

            model.IsActive = !model.IsActive;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _bankData.UpdateRecordStatus(model);
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
            UserModel user = await GetLoggedInUser();

            model.IsArchived = true;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _bankData.ArchiveRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<BankBalanceSumDTO> GetBankBalancesSum()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            BankBalanceSumDTO result = await _bankData.GetBankBalancesSum(user.Id);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<BankModel>> GetRecordsActive()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<BankModel> results = await _bankData.GetRecordsActive(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return await _authProvider.GetUserFromAuth(_userData);
    }
}
