namespace MainApp.Services;

public interface ILocationExpenseService<T>
{
    public Task<List<LocationExpenseDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
}
