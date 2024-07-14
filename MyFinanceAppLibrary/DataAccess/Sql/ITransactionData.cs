namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITransactionData<T> : IBaseData<T>
{
    Task ArchiveRecordCredit(T model);
    Task ArchiveRecordDebit(T model);
    Task ArchiveRecordTransfer(T model);
    Task CreateRecordCredit(T model);
    Task CreateRecordDebit(T model);
    Task CreateRecordTransfer(T model);
    Task<List<TransactionIOGraphByMonthDTO>> GetIOByDateRangeGroupByMonth(string userId, DateTimeRange dateTimeRange);
    Task<List<TransactionIOGraphByDayDTO>> GetIOByDateRangeGroupByDay(string userId, DateTimeRange dateTimeRange);
    Task<List<TransactionListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task<List<TransactionDetailsDTO>> GetRecordsDetailsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task<List<TransactionIOLast3MonthsGraphDTO>> GetRecordsLast3Months(string userId);
}
