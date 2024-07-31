namespace MainApp.Services;

public interface ITransactionService<T> : IBaseService<T>
{
    public Task CreateRecordCredit(T model);
    public Task CreateRecordDebit(T model);
    public Task CreateRecordTransfer(T model);
    public Task<List<TransactionIOGraphByMonthDTO>> GetIOByDateRangeGroupByMonth(DateTimeRange dateTimeRange);
    public Task<List<TransactionIOGraphByDayDTO>> GetIOByDateRangeGroupByDay(DateTimeRange dateTimeRange);
    public Task<List<TransactionByCategoryGroupDTO>> GetRecordsListView(FilterTransactionDTO filter);
    public Task<List<TransactionCalendarDTO>> GetRecordsCalendarView(FilterTransactionDTO filter);
    public Task<List<TransactionDetailsDTO>> GetRecordsDateView(DateTimeRange dateTimeRange);
    public Task<List<TransactionIOLast3MonthsGraphDTO>> GetRecordsLast3Months();
}
