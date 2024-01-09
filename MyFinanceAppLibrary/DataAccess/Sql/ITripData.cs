namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITripData<T> : IBaseData<T>
{
    Task<List<TripListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task UpdateRecordPayStatus(T model);
    Task UpdateRecordTripCategory(T model);
}
