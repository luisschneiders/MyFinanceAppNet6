namespace MainApp.Services;

public interface IShiftService<T> : IBaseService<T>
{
    public Task<List<ShiftListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    public Task SaveRecord(T model);
}
