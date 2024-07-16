namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    Task<List<TimesheetByCompanyGroupDTO>> GetRecordsListView(FilterTimesheetDTO filter);
    Task<List<TimesheetCalendarDTO>> GetRecordsCalendarView(FilterTimesheetDTO filter);
    Task<decimal> GetSumByDateRange();
    Task<double> GetSumTotalHours();
    Task UpdateRecordPayStatus(T model);
}
