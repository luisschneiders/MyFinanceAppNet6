namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    Task<List<TimesheetModelListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<TimesheetModelListDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, CompanyModel model);
    Task<decimal> GetSumTotalAwaiting();
    Task<decimal> GetSumTotalPaid();
    Task<double> GetSumTotalHours();
    Task UpdateRecordPayStatus(T model);
}
