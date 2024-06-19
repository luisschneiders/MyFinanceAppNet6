namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IExpenseData<T> : IBaseData<T>
{
    Task<List<ExpenseListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task<List<ExpenseDetailsDTO>> GetRecordsDetailsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months(string userId);
    Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years(string userId);
    Task<List<ExpenseAmountHistoryDTO>> GetAmountHistory(string userId);
    Task<List<ExpenseTop5DTO>> GetTop5ExpensesByDateRange(string userId, DateTimeRange dateTimeRange);
    Task<List<ExpenseListGroupByMonthDTO>> GetRecordsGroupByMonth(string userId, DateTimeRange dateTimeRange);
}
