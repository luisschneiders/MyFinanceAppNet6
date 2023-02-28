namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IExpenseData<T> : IBaseData<T>
{
    Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
}
