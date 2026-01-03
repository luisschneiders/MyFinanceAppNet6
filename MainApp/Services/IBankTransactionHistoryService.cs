namespace MainApp.Services;

public interface IBankTransactionHistoryService<T> : IBaseService<T>
{
    public Task<List<BankTransactionHistoryListDTO>> GetRecordsByDateRange(string bankId, DateTimeRange dateTimeRange);
}
