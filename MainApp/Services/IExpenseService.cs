namespace MainApp.Services;

public interface IExpenseService<T> : IBaseService<T>
{
    Task<decimal> GetRecordsByDateRangeSum();
    Task<List<ExpenseByCategoryGroupDTO>> GetRecordsListView(FilterExpenseDTO filter);
    Task<List<ExpenseCalendarDTO>> GetRecordsCalendarView(FilterExpenseDTO filter);
    Task<List<ExpenseDetailsDTO>> GetRecordsDateView(DateTimeRange dateTimeRange);
    Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months();
    Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years();
    Task<List<ExpenseAmountHistoryDTO>> GetAmountHistory();
    Task<GoogleMapStaticModel> GetLocationExpense(DateTimeRange dateTimeRange, MapMarkerColor mapMarkerColor, MapSize mapSizeWidth, MapSize mapSizeHeight, MapScale scale);
    Task<List<ExpenseTop5DTO>> GetRecordsTop5ByDate(DateTimeRange dateTimeRange);
    Task<List<ExpenseListGroupByMonthDTO>> GetRecordsGroupByMonth(DateTimeRange dateTimeRange);
}
