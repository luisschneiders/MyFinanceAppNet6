namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankTransactionHistoryData<T> : IBaseData<T>
{
    public Task<List<BankTransactionHistoryListDTO>> GetRecordsByDateRange(string userId, string bankId, DateTimeRange dateTimeRange);
}
