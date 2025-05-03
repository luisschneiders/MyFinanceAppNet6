namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ILocationExpenseData<T>
{
    public Task<List<LocationModel>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
}
