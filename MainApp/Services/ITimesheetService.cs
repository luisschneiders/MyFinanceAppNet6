namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    Task<List<TimesheetListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<TimesheetListDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, CompanyModel model);
    Task<decimal> GetSumTotalAwaiting();
    Task<decimal> GetSumTotalPaid();
    Task<double> GetSumTotalHours();
    Task UpdateRecordPayStatus(T model);
}
