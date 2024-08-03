namespace MainApp.Services;

public interface ITripService<T> : IBaseService<T>
{
    public Task<List<TripByVehicleGroupDTO>> GetRecordsListView(MultiFilterTripDTO filter);
    public Task<decimal> GetSumByDateRange();
    public Task UpdateRecordPayStatus(T model);
    public Task UpdateRecordTripCategory(T model);
}
