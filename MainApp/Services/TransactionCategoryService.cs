using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class TransactionCategoryService : ITransactionCategoryService<TransactionCategoryModel>
{
    [Inject]
    private ITransactionCategoryData<TransactionCategoryModel> _transactionCategoryData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    public TransactionCategoryService(ITransactionCategoryData<TransactionCategoryModel> transactionCategoryData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _transactionCategoryData = transactionCategoryData;
        _userData = userData;
        _authProvider = authProvider;
    }

    // TODO: Add pagination capabilities
    public async Task<List<TransactionCategoryModel>> GetRecords()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<TransactionCategoryModel> results = await _transactionCategoryData.GetRecords(user.Id);
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
            List<TransactionCategoryModel> results = await _transactionCategoryData.GetRecords(user.Id);

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

    public async Task<List<TransactionCategoryModel>> GetSearchResults(string search)
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<TransactionCategoryModel> results = await _transactionCategoryData.GetSearchResults(user.Id, search);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<TransactionCategoryModel> GetRecordById(string modelId)
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            TransactionCategoryModel result = await _transactionCategoryData.GetRecordById(user.Id, modelId);
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
        var lastInsertedId = await _transactionCategoryData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task CreateRecord(TransactionCategoryModel model)
    {
        try
        {
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _transactionCategoryData.CreateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecord(TransactionCategoryModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _transactionCategoryData.UpdateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(TransactionCategoryModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            UserModel user = await GetLoggedInUser();

            model.IsActive = !model.IsActive;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _transactionCategoryData.UpdateRecordStatus(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveRecord(TransactionCategoryModel model)
    {
        try
        {
            UserModel user = await GetLoggedInUser();

            model.IsArchived = true;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _transactionCategoryData.ArchiveRecord(model);
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

    public async Task<List<TransactionCategoryModel>> GetRecordsActive()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<TransactionCategoryModel> results = await _transactionCategoryData.GetRecordsActive(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
