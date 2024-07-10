namespace MainApp.Services;

public interface ITransactionService<T> : IBaseService<T>
{
    Task CreateRecordCredit(T model);
    Task CreateRecordDebit(T model);
    Task CreateRecordTransfer(T model);
    Task<List<TransactionIOGraphByMonthDTO>> GetIOByDateRangeGroupByMonth(DateTimeRange dateTimeRange);
    Task<List<TransactionIOGraphByDayDTO>> GetIOByDateRangeGroupByDay(DateTimeRange dateTimeRange);
    Task<List<TransactionByCategoryGroupDTO>> GetRecordsListView(FilterTransactionDTO filter);
    Task<List<TransactionCalendarDTO>> GetRecordsCalendarView(FilterTransactionDTO filter);
    Task<List<TransactionIOLast3MonthsGraphDTO>> GetRecordsLast3Months();
}
