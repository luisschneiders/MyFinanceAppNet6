namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITransactionData<T> : IBaseData<T>
{
    public Task ArchiveRecordCredit(T model);
    public Task ArchiveRecordDebit(T model);
    public Task ArchiveRecordTransfer(T model);
    public Task CreateRecordCredit(T model);
    public Task CreateRecordDebit(T model);
    public Task CreateRecordTransfer(T model);
    public Task<List<TransactionIOGraphByMonthDTO>> GetIOByDateRangeGroupByMonth(string userId, DateTimeRange dateTimeRange);
    public Task<List<TransactionIOGraphByDayDTO>> GetIOByDateRangeGroupByDay(string userId, DateTimeRange dateTimeRange);
    public Task<List<TransactionListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task<List<TransactionDetailsDTO>> GetRecordsDetailsByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task<List<TransactionIOLast3MonthsGraphDTO>> GetRecordsLast3Months(string userId);
}
