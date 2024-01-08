namespace MainApp.Services;

public interface ITripService<T> : IBaseService<T>
{
    Task<List<TripListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<decimal> GetSumByDateRange();
    Task UpdateRecordPayStatus(T model);
}
