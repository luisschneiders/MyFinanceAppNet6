namespace MyFinanceAppLibrary.DataAccess.Sql;

public class TransactionCategoryData : ITransactionCategoryData<TransactionCategoryModel>
{
    private readonly IDataAccess _dataAccess;

    public TransactionCategoryData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(TransactionCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransactionCategory_Archive",
                new
                {
                    transactionCategoryId = model.Id,
                    transactionCategoryIsArchived = model.IsArchived,
                    transactionCategoryUpdatedBy = model.UpdatedBy,
                    transactionCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(TransactionCategoryModel model)
    {
        try
        {
            await _dataAccess.LoadData<TransactionCategoryModel, dynamic>(
                "myfinancedb.spTransactionCategory_Create",
                new
                {
                    transactionCategoryDescription = model.Description,
                    transactionCategoryActionType = model.ActionType,
                    transactionCategoryUpdatedBy = model.UpdatedBy,
                    transactionCategoryCreatedAt = model.CreatedAt,
                    transactionCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _dataAccess.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task<TransactionCategoryModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<TransactionCategoryModel, dynamic>(
                "myfinancedb.spTransactionCategory_GetById",
                new { userId = userId, transactionCategoryId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TransactionCategoryModel>> GetRecords(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<TransactionCategoryModel, dynamic>(
                "myfinancedb.spTransactionCategory_GetAll",
                new { userId = userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TransactionCategoryModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TransactionCategoryModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<TransactionCategoryModel, dynamic>(
                "myfinancedb.spTransactionCategory_GetSearchResults",
                new
                {
                    userId = userId,
                    searchTransactionCategory = search,
                },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecord(TransactionCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransactionCategory_Update",
                new
                {
                    transactionCategoryId = model.Id,
                    transactionCategoryDescription = model.Description,
                    transactionCategoryActionType = model.ActionType,
                    transactionCategoryIsActive = model.IsActive,
                    transactionCategoryUpdatedBy = model.UpdatedBy,
                    transactionCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(TransactionCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransactionCategory_UpdateStatus",
                new
                {
                    transactionCategoryId = model.Id,
                    transactionCategoryIsActive = model.IsActive,
                    transactionCategoryUpdatedBy = model.UpdatedBy,
                    transactionCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
