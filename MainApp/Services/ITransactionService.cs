namespace MainApp.Services;

public interface ITransactionService<T> : IBaseService<T>
{
    Task CreateRecordCredit(T model);
    Task CreateRecordDebit(T model);
    Task CreateRecordTransfer(T model);
    Task<List<TransactionIOGraphByMonthDTO>> GetIOByDateRangeGroupByMonth(DateTimeRange dateTimeRange);
    Task<List<TransactionIOGraphByDayDTO>> GetIOByDateRangeGroupByDay(DateTimeRange dateTimeRange);
    Task<List<TransactionListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<TransactionByCategoryGroupDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, FilterTransactionDTO filterTransactionDTO);
    Task<List<TransactionByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
    Task<List<TransactionIOLast3MonthsGraphDTO>> GetRecordsLast3Months();
}
