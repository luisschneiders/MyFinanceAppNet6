namespace MyFinanceAppLibrary.DataAccess.Sql;

public class TransactionTypeData : ITransactionTypeData<TransactionTypeModel>
{
    private readonly IDataAccess _dataAccess;

    public TransactionTypeData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(TransactionTypeModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransactionType_Archive",
                new
                {
                    transactionTypeId = model.Id,
                    transactionTypeIsArchived = model.IsArchived,
                    transactionTypeUpdatedBy = model.UpdatedBy,
                    transactionTypeUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(TransactionTypeModel model)
    {
        try
        {
            await _dataAccess.LoadData<TransactionTypeModel, dynamic>(
                "myfinancedb.spTransactionType_Create",
                new
                {
                    transactionTypeDescription = model.Description,
                    transactionTypeAction = model.ActionType,
                    transactionTypeUpdatedBy = model.UpdatedBy,
                    transactionTypeCreatedAt = model.CreatedAt,
                    transactionTypeUpdatedAt = model.UpdatedAt,
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

    public async Task<TransactionTypeModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<TransactionTypeModel, dynamic>(
                "myfinancedb.spTransactionType_GetById",
                new { userId = userId, transactionTypeId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TransactionTypeModel>> GetRecords(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<TransactionTypeModel, dynamic>(
                "myfinancedb.spTransactionType_GetAll",
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

    public async Task<List<TransactionTypeModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<TransactionTypeModel, dynamic>(
                "myfinancedb.spTransactionType_GetSearchResults",
                new
                {
                    userId = userId,
                    searchTransactionType = search,
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

    public async Task UpdateRecord(TransactionTypeModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransactionType_Update",
                new
                {
                    transactionTypeId = model.Id,
                    transactionTypeDescription = model.Description,
                    transactionTypeAction = model.ActionType,
                    transactionTypeIsActive = model.IsActive,
                    transactionTypeUpdatedBy = model.UpdatedBy,
                    transactionTypeUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(TransactionTypeModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransactionType_UpdateStatus",
                new
                {
                    transactionTypeId = model.Id,
                    transactionTypeIsActive = model.IsActive,
                    transactionTypeUpdatedBy = model.UpdatedBy,
                    transactionTypeUpdatedAt = model.UpdatedAt,
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
