namespace MainApp.Services;

public interface ILocationExpenseService<T>
{
    public Task<List<LocationModel>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
}
