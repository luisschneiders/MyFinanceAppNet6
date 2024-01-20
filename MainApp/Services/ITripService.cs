namespace MainApp.Services;

public interface ITripService<T> : IBaseService<T>
{
    Task<List<TripListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<TripByVehicleGroupDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, FilterTripDTO filterTripDTO);
    Task<List<TripByVehicleGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
    Task<decimal> GetSumByDateRange();
    Task UpdateRecordPayStatus(T model);
    Task UpdateRecordTripCategory(T model);
}
