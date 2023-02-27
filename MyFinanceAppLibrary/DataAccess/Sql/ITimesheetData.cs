namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITimesheetData<T> : IBaseData<T>
{
    Task<List<TimesheetModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    Task UpdateRecordPayStatus(T model);
}
