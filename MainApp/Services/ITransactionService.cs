namespace MainApp.Services;

public interface ITransactionService<T> : IBaseService<T>
{
    Task CreateRecordCredit(T model);
    Task CreateRecordDebit(T model);
    Task CreateRecordTransfer(T model);
    Task<List<TransactionIOGraphByDateDTO>> GetIOByDateRange(DateTimeRange dateTimeRange);
    Task<List<TransactionListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<TransactionByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
}
