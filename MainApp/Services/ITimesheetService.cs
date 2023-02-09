namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    Task<List<TimesheetModel>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel);
    Task UpdateRecordPayStatus(T model);
}
