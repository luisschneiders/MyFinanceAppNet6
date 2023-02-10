namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITimesheetData<T> : IBaseData<T>
{
    Task<List<TimesheetModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRangeModel dateTimeRangeModel);
    Task UpdateRecordPayStatus(T model);
}
