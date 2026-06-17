
namespace MyFinanceAppLibrary.DataAccess.Sql;

public class BankTransactionHistoryData : IBankTransactionHistoryData<BankTransactionHistoryModel>
{
    private readonly IDataAccess _dataAccess;

    public BankTransactionHistoryData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    public Task ArchiveRecord(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateRecord(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public Task<BankTransactionHistoryModel> GetRecordById(string userId, string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BankTransactionHistoryModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BankTransactionHistoryModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BankTransactionHistoryListDTO>> GetRecordsByBankLoadMore(string userId, ulong bankId, ulong? lastId, LoadMore loadMore)
    {
        try
        {
            var results = await _dataAccess.LoadData<BankTransactionHistoryListDTO, dynamic>(
                "myfinancedb.spBankTransactionHistory_GetRecordsByBank_LoadMore",
                new
                {
                    userId,
                    bankId,
                    lastId,
                    loadMore
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

    public Task<List<BankTransactionHistoryModel>> GetSearchResults(string userId, string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }
}
