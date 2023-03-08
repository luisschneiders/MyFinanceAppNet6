namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IExpenseData<T> : IBaseData<T>
{
    Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months(string userId);
}
