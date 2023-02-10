namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    Task<List<TimesheetModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel);
    Task UpdateRecordPayStatus(T model);
}
