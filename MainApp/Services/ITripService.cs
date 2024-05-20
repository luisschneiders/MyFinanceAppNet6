namespace MainApp.Services;

public interface ITripService<T> : IBaseService<T>
{
    Task<List<TripByVehicleGroupDTO>> GetRecordsListView(FilterTripDTO filter);
    Task<decimal> GetSumByDateRange();
    Task UpdateRecordPayStatus(T model);
    Task UpdateRecordTripCategory(T model);
}
