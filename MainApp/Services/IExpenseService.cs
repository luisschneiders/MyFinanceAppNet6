namespace MainApp.Services;

public interface IExpenseService<T> : IBaseService<T>
{
    Task<List<ExpenseListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<decimal> GetRecordsByDateRangeSum();
    Task<List<ExpenseByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
    Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months();
    Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years();
    Task<List<ExpenseAmountHistoryDTO>> GetAmountHistory();
}
