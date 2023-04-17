namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IExpenseData<T> : IBaseData<T>
{
    Task<List<ExpenseListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months(string userId);
    Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years(string userId);
    Task<List<ExpenseAmountHistoryDTO>> GetAmountHistoryDTO(string userId);
}
