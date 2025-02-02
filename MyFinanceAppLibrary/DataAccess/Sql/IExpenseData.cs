namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IExpenseData<T> : IBaseData<T>
{
    public Task<List<ExpenseListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task<List<ExpenseDetailsDTO>> GetRecordsDetailsByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months(string userId);
    public Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years(string userId);
    public Task<List<ExpenseAmountHistoryDTO>> GetAmountHistory(string userId);
    public Task<List<ExpenseTop5DTO>> GetTop5ExpensesByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task<List<ExpenseListGroupByMonthDTO>> GetRecordsGroupByMonth(string userId, DateTimeRange dateTimeRange);
}
