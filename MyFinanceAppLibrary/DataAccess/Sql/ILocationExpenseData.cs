namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ILocationExpenseData<T>
{
    Task<List<LocationExpenseDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
}
