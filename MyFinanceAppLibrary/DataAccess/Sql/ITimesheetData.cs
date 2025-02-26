namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITimesheetData<T> : IBaseData<T>
{
    public Task<List<TimesheetListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange);
    public Task UpdateRecordPayStatus(T model);
}
