namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITimesheetData<T> : IBaseData<T>
{
    Task<List<TimesheetModel>> GetRecordsByDateRange(string userId, DateTimeRangeModel dateTimeRangeModel);
    Task UpdateRecordPayStatus(T model);
}
