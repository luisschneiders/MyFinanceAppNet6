namespace MainApp.Services;

public interface ITimesheetService<T> : IBaseService<T>
{
    public Task<List<TimesheetByCompanyGroupDTO>> GetRecordsListView(MultiFilterTimesheetDTO filter);
    public Task<List<TimesheetCalendarDTO>> GetRecordsCalendarView(MultiFilterTimesheetDTO filter);
    public Task<TimesheetTotal> GetTotals();
    public Task UpdateRecordPayStatus(T model);
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
}
