namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITimesheetData<T> : IBaseData<T>
{
    Task UpdateRecordPayStatus(T model);
}
