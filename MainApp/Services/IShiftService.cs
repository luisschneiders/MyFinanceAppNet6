namespace MainApp.Services;

public interface IShiftService<T> : IBaseService<T>
{
    public Task<List<ShiftListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    public Task SaveAvailability(T model);
    public Task SaveShift(T model);
}
