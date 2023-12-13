namespace MainApp.Services;

public interface IExpenseService<T> : IBaseService<T>
{
    Task<List<ExpenseListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    // Task<List<ExpenseByCategoryGroupDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, ExpenseCategoryModel expenseCategoryModel);
    Task<List<ExpenseByCategoryGroupDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, FilterExpenseDTO filterExpenseDTO);
    Task<decimal> GetRecordsByDateRangeSum();
    Task<List<ExpenseByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
    Task<List<ExpenseCalendarDTO>> GetRecordsCalendarView(DateTimeRange dateTimeRange);
    Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months();
    Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years();
    Task<List<ExpenseAmountHistoryDTO>> GetAmountHistory();
    Task<GoogleMapStaticModel> GetLocationExpense(DateTimeRange dateTimeRange, MapMarkerColor mapMarkerColor, MapSize mapSizeWidth, MapSize mapSizeHeight, MapScale scale);
}
