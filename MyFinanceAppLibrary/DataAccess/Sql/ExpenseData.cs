namespace MyFinanceAppLibrary.DataAccess.Sql;

public class ExpenseData : IExpenseData<ExpenseModel>
{
    private readonly IDataAccess _dataAccess;

    public ExpenseData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task ArchiveRecord(ExpenseModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateRecord(ExpenseModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public Task<ExpenseModel> GetRecordById(string userId, string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRangeModel dateTimeRangeModel)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModel>> GetSearchResults(string userId, string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(ExpenseModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(ExpenseModel model)
    {
        throw new NotImplementedException();
    }
}
