namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    public Task<List<TimesheetByCompanyGroupDTO>> GetRecordsListView(MultiFilterTimesheetDTO filter);
    public Task<List<TimesheetCalendarDTO>> GetRecordsCalendarView(MultiFilterTimesheetDTO filter);
    public Task<decimal> GetSumByDateRange();
    public Task<double> GetSumTotalHours();
    public Task UpdateRecordPayStatus(T model);
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
}
