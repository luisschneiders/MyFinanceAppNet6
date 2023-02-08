namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    Task UpdateRecordPayStatus(T model);
}
