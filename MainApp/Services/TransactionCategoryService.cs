using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class TransactionCategoryService : ITransactionCategoryService
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
    public async Task<List<TransactionCategoryModel>> GetTransactionCategories()
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

    public async Task<TransactionCategoryModel> GetTransactionCategoryById(string modelId)
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

    public async Task CreateTransactionCategory(TransactionCategoryModel model)
    {
        try
        {
            var user = await GetLoggedInUser();
            TransactionCategoryModel newTransactionCategory = new()
            {
                Description = model.Description,
                ActionType = model.ActionType,
                UpdatedBy = user.Id
            };

            await _transactionCategoryData.CreateRecord(newTransactionCategory);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateTransactionCategory(TransactionCategoryModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();
            TransactionCategoryModel newTransactionCategory = new()
            {
                Id = model.Id,
                Description = model.Description,
                ActionType = model.ActionType,
                IsActive = model.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _transactionCategoryData.UpdateRecord(newTransactionCategory);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateTransactionCategoryStatus(TransactionCategoryModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();

            TransactionCategoryModel transactionCategoryStatusUpdate = model;
            transactionCategoryStatusUpdate.IsActive = !model.IsActive;
            transactionCategoryStatusUpdate.UpdatedBy = user.Id;
            transactionCategoryStatusUpdate.UpdatedAt = DateTime.Now;

            await _transactionCategoryData.UpdateRecordStatus(transactionCategoryStatusUpdate);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveTransactionCategory(TransactionCategoryModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            TransactionCategoryModel transactionCategoryStatusUpdate = model;
            transactionCategoryStatusUpdate.IsArchived = true;
            transactionCategoryStatusUpdate.UpdatedBy = user.Id;
            transactionCategoryStatusUpdate.UpdatedAt = DateTime.Now;

            await _transactionCategoryData.ArchiveRecord(transactionCategoryStatusUpdate);
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
