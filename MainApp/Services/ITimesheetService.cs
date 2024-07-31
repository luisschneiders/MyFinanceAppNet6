namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    public Task<List<TimesheetByCompanyGroupDTO>> GetRecordsListView(FilterTimesheetDTO filter);
    public Task<List<TimesheetCalendarDTO>> GetRecordsCalendarView(FilterTimesheetDTO filter);
    public Task<decimal> GetSumByDateRange();
    public Task<double> GetSumTotalHours();
    public Task UpdateRecordPayStatus(T model);
}
