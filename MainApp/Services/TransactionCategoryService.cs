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

    private UserModel _loggedInUser { get; set; } = new();

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
            var user = await GetLoggedInUser();
            List<TransactionCategoryModel> results = await _transactionCategoryData.GetRecords(user.Id);
            return results;
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
            var user = await GetLoggedInUser();
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
            var user = await GetLoggedInUser();
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
            var user = await GetLoggedInUser();
            TransactionCategoryModel recordModel = new()
            {
                Description = model.Description,
                ActionType = model.ActionType,
                UpdatedBy = user.Id
            };

            await _transactionCategoryData.CreateRecord(recordModel);
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
            var user = await GetLoggedInUser();
            TransactionCategoryModel recordModel = new()
            {
                Id = model.Id,
                Description = model.Description,
                ActionType = model.ActionType,
                IsActive = model.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _transactionCategoryData.UpdateRecord(recordModel);
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
            var user = await GetLoggedInUser();

            TransactionCategoryModel recordModel = model;
            recordModel.IsActive = !model.IsActive;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _transactionCategoryData.UpdateRecordStatus(recordModel);
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
            var user = await GetLoggedInUser();

            TransactionCategoryModel recordModel = model;
            recordModel.IsArchived = true;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _transactionCategoryData.ArchiveRecord(recordModel);
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

    public async Task<List<TransactionCategoryModel>> GetRecordsActive()
    {
        try
        {
            var user = await GetLoggedInUser();
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
