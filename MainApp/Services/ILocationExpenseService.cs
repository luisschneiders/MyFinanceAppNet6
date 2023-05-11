namespace MainApp.Services;

public interface ILocationExpenseService<T>
{
    Task<List<LocationExpenseDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
}
