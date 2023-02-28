namespace MainApp.Services;

public interface ITripService<T> : IBaseService<T>
{
    Task<List<TripModelListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<decimal> GetSumByDateRange();
}
