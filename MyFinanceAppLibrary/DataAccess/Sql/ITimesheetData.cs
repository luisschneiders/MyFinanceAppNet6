namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITimesheetData<T> : IBaseData<T>
{
    Task<List<TimesheetListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task UpdateRecordPayStatus(T model);
}
