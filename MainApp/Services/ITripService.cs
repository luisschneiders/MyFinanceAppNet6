namespace MainApp.Services;

public interface ITripService<T> : IBaseService<T>
{
    Task<List<TripModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel);
    Task<decimal> GetSumByDateRange();
}
