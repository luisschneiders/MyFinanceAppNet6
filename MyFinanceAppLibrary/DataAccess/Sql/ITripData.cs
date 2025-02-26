namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITripData<T> : IBaseData<T>
{
    public Task<List<TripListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task UpdateRecordPayStatus(T model);
    public Task UpdateRecordTripCategory(T model);
}
