namespace MyFinanceAppLibrary.DataAccess.Sql;

public class TransactionData : ITransactionData<TransactionModel>
{
    private readonly IDataAccess _dataAccess;

    public TransactionData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task ArchiveRecord(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    public async Task ArchiveRecordCredit(TransactionModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransaction_ArchiveCredit",
                new
                {
                    transactionId = model.Id,
                    transactionIsActive = model.IsActive,
                    transactionIsArchived = model.IsArchived,
                    transactionUpdatedBy = model.UpdatedBy,
                    transactionUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveRecordDebit(TransactionModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransaction_ArchiveDebit",
                new
                {
                    transactionId = model.Id,
                    transactionIsActive = model.IsActive,
                    transactionIsArchived = model.IsArchived,
                    transactionUpdatedBy = model.UpdatedBy,
                    transactionUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveRecordTransfer(TransactionModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransaction_ArchiveTransfer",
                new
                {
                    transactionId = model.Id,
                    transactionIsActive = model.IsActive,
                    transactionIsArchived = model.IsArchived,
                    transactionUpdatedBy = model.UpdatedBy,
                    transactionUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task CreateRecord(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    public async Task CreateRecordCredit(TransactionModel model)
    {
        try
        {
            await _dataAccess.LoadData<TransactionModel, dynamic>(
                "myfinancedb.spTransaction_CreateCredit",
                new
                {
                    transactionTDate = model.TDate,
                    transactionFromBank = model.FromBank,
                    transactionTCategoryId = model.TCategoryId,
                    transactionAction = model.Action,
                    transactionLabel = model.Label,
                    transactionAmount = model.Amount,
                    transactionComments = model.Comments,
                    transactionUpdatedBy = model.UpdatedBy,
                    transactionCreatedAt = model.CreatedAt,
                    transactionUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecordDebit(TransactionModel model)
    {
        try
        {
            await _dataAccess.LoadData<TransactionModel, dynamic>(
                "myfinancedb.spTransaction_CreateDebit",
                new
                {
                    transactionTDate = model.TDate,
                    transactionFromBank = model.FromBank,
                    transactionTCategoryId = model.TCategoryId,
                    transactionAction = model.Action,
                    transactionLabel = model.Label,
                    transactionAmount = model.Amount,
                    transactionComments = model.Comments,
                    transactionUpdatedBy = model.UpdatedBy,
                    transactionCreatedAt = model.CreatedAt,
                    transactionUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecordTransfer(TransactionModel model)
    {
        try
        {
            await _dataAccess.LoadData<TransactionModel, dynamic>(
                "myfinancedb.spTransaction_CreateTransfer",
                new
                {
                    transactionTDate = model.TDate,
                    transactionFromBank = model.FromBank,
                    transactionToBank = model.ToBank, // For transfers only
                    transactionTCategoryId = model.TCategoryId,
                    transactionLabel = model.Label,
                    transactionAmount = model.Amount,
                    transactionComments = model.Comments,
                    transactionUpdatedBy = model.UpdatedBy,
                    transactionCreatedAt = model.CreatedAt,
                    transactionUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public async Task<TransactionModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<TransactionModel, dynamic>(
                "myfinancedb.spTransaction_GetById",
                new { userId = userId, transactionId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TransactionModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TransactionModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TransactionModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange)
    {
        try
        {
            var results = await _dataAccess.LoadData<TransactionModelListDTO, dynamic>(
                "myfinancedb.spTransaction_GetRecordsByDateRange",
                new
                {
                    userId = userId,
                    startDate = dateTimeRange.Start,
                    endDate = dateTimeRange.End
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

    public Task<List<TransactionModel>> GetSearchResults(string userId, string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(TransactionModel model)
    {
        throw new NotImplementedException();
    }
}
