namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    Task<List<TimesheetListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange);
    Task<List<TimesheetByCompanyGroupDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, FilterTimesheetDTO filterTimesheetDTO);
    Task<List<TimesheetByCompanyGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange);
    Task<decimal> GetSumByDateRange();
    // Task<decimal> GetSumTotalAwaiting();
    // Task<decimal> GetSumTotalPaid();
    Task<double> GetSumTotalHours();
    Task UpdateRecordPayStatus(T model);
}
