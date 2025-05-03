namespace MainApp.Services;

public interface IExpenseService<T> : IBaseService<T>
{
    public Task<decimal> GetRecordsByDateRangeSum();
    public Task<List<ExpenseByCategoryGroupDTO>> GetRecordsListView(MultiFilterExpenseDTO filter);
    public Task<List<ExpenseCalendarDTO>> GetRecordsCalendarView(MultiFilterExpenseDTO filter);
    public Task<List<ExpenseDetailsDTO>> GetRecordsDateView(DateTimeRange dateTimeRange);
    public Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months();
    public Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years();
    public Task<List<ExpenseAmountHistoryDTO>> GetAmountHistory();
    public Task<GoogleMapStaticModel> GetLocationExpense(DateTimeRange dateTimeRange, MapMarkerColor mapMarkerColor, MapSize mapSizeWidth, MapSize mapSizeHeight, MapScale scale);
    public Task<List<LocationModel>> GetLocationExpenseList(DateTimeRange dateTimeRange);
    public Task<List<ExpenseTop5DTO>> GetRecordsTop5ByDate(DateTimeRange dateTimeRange);
    public Task<List<ExpenseListGroupByMonthDTO>> GetRecordsGroupByMonth(DateTimeRange dateTimeRange);
    public Task<string> GetLocalStorageViewType();
    public Task SetLocalStorageViewType(string view);
}
