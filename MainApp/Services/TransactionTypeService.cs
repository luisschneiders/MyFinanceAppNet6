using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class TransactionTypeService : ITransactionTypeService
{
    [Inject]
    private ITransactionTypeData<TransactionTypeModel> _transactionTypeData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public TransactionTypeService(ITransactionTypeData<TransactionTypeModel> transactionTypeData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _transactionTypeData = transactionTypeData;
        _userData = userData;
        _authProvider = authProvider;
    }

    // TODO: Add pagination capabilities
    public async Task<List<TransactionTypeModel>> GetTransactionTypes()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TransactionTypeModel> results = await _transactionTypeData.GetRecords(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TransactionTypeModel>> GetSearchResults(string search)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TransactionTypeModel> results = await _transactionTypeData.GetSearchResults(user.Id, search);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<TransactionTypeModel> GetTransactionTypeById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            TransactionTypeModel result = await _transactionTypeData.GetRecordById(user.Id, modelId);
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
        var lastInsertedId = await _transactionTypeData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task CreateTransactionType(TransactionTypeModel model)
    {
        try
        {
            var user = await GetLoggedInUser();
            TransactionTypeModel newTransactionType = new()
            {
                Description = model.Description,
                ActionType = model.ActionType,
                UpdatedBy = user.Id
            };

            await _transactionTypeData.CreateRecord(newTransactionType);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateTransactionType(TransactionTypeModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();
            TransactionTypeModel newTransactionType = new()
            {
                Id = model.Id,
                Description = model.Description,
                ActionType = model.ActionType,
                IsActive = model.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _transactionTypeData.UpdateRecord(newTransactionType);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateTransactionTypeStatus(TransactionTypeModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();

            TransactionTypeModel transactionTypeStatusUpdate = model;
            transactionTypeStatusUpdate.IsActive = !model.IsActive;
            transactionTypeStatusUpdate.UpdatedBy = user.Id;
            transactionTypeStatusUpdate.UpdatedAt = DateTime.Now;

            await _transactionTypeData.UpdateRecordStatus(transactionTypeStatusUpdate);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveTransactionType(TransactionTypeModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            TransactionTypeModel transactionTypeStatusUpdate = model;
            transactionTypeStatusUpdate.IsArchived = true;
            transactionTypeStatusUpdate.UpdatedBy = user.Id;
            transactionTypeStatusUpdate.UpdatedAt = DateTime.Now;

            await _transactionTypeData.ArchiveRecord(transactionTypeStatusUpdate);
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
