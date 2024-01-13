namespace MainApp.Services;

public interface ITripService<T> : IBaseService<T>
{
    Task<List<TripListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<TripListDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, FilterTripDTO filterTripDTO);
    Task<decimal> GetSumByDateRange();
    Task UpdateRecordPayStatus(T model);
    Task UpdateRecordTripCategory(T model);
}
