namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ILocationExpenseData<T>
{
    public Task<List<LocationExpenseDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
}
