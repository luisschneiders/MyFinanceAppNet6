namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IShiftData<T> : IBaseData<T>
{
    public Task<List<ShiftListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task SaveRecord(T model);
}
