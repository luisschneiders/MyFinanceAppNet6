namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITripData<T> : IBaseData<T>
{
    Task<List<TripModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
}
