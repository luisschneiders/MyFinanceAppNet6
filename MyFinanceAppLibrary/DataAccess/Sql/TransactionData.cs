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

    public Task CreateRecord(TransactionModel model)
    {
        throw new NotImplementedException();
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
