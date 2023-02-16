namespace MyFinanceAppLibrary.DataAccess.Sql;

public class TransactionData : ITransactionData<TransactionModel>
{
    private readonly IDataAccess _dataAccess;

    public TransactionData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(TransactionModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTransaction_Archive",
                new
                {
                    transactionId = model.Id,
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
                    transactionTCategoryType = model.TCategoryType,
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
                    transactionTCategoryType = model.TCategoryType,
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
                    transactionTCategoryType = model.TCategoryType,
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

    public Task<TransactionModel> GetRecordById(string userId, string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TransactionModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TransactionModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
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
