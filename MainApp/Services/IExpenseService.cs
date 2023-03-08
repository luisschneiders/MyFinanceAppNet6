namespace MainApp.Services;

public interface IExpenseService<T> : IBaseService<T>
{
    Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<decimal> GetRecordsByDateRangeSum();
    Task<List<ExpenseModelByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
    Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months();
}
