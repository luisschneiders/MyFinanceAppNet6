using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class TimesheetService : ITimesheetService<TimesheetModel>
{
    [Inject]
    private ITimesheetData<TimesheetModel> _timesheetData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();
    private List<TimesheetListDTO> _recordsByDateRange { get; set; } = new();
    private decimal _timesheetByDateRangeSum { get; set; } = 0;

    public TimesheetService(ITimesheetData<TimesheetModel> timesheetData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _timesheetData = timesheetData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task ArchiveRecord(TimesheetModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.IsArchived = true;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _timesheetData.ArchiveRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(TimesheetModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _timesheetData.CreateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _timesheetData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task<TimesheetModel> GetRecordById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            TimesheetModel result = await _timesheetData.GetRecordById(user.Id, modelId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TimesheetModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public async Task<List<TimesheetModel>> GetRecordsActive()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TimesheetModel> results = await _timesheetData.GetRecordsActive(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TimesheetByCompanyGroupDTO>> GetRecordsListView(FilterTimesheetDTO filter)
    {
        try
        {
            List<TimesheetListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<TimesheetListDTO> recordsFiltered = new();
            List<TimesheetByCompanyGroupDTO> results = new();

            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                results = await SetRecordsListView(recordsFiltered);
            }
            else
            {
                results = await SetRecordsListView(records);
            }

            return await Task.FromResult(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TimesheetCalendarDTO>> GetRecordsCalendarView(FilterTimesheetDTO filter)
    {
        try
        {
            List<TimesheetListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<TimesheetListDTO> recordsFiltered = new();
            List<TimesheetCalendarDTO> calendarData = new();
            
            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                _timesheetByDateRangeSum = recordsFiltered.Sum(t => t.TotalAmount);
                calendarData = await SetRecordsCalendarView(recordsFiltered);
            }
            else
            {
                _timesheetByDateRangeSum = records.Sum(t => t.TotalAmount);
                calendarData = await SetRecordsCalendarView(records);
            }

            return calendarData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<decimal> GetSumByDateRange()
    {
        try
        {
            return await Task.FromResult(_timesheetByDateRangeSum);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<double> GetSumTotalHours()
    {
        var totalHours = _recordsByDateRange.Sum(s => s.HoursWorked.TotalHours);
        return await Task.FromResult(totalHours);
    }

    public Task<List<TimesheetModel>> GetSearchResults(string search)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRecord(TimesheetModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _timesheetData.UpdateRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(TimesheetModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.IsActive = !model.IsActive;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _timesheetData.UpdateRecordStatus(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordPayStatus(TimesheetModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _timesheetData.UpdateRecordPayStatus(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<TimesheetListDTO>> SetRecordsFilter(FilterTimesheetDTO filter)
    {
        try
        {
            if (filter.CompanyId > 0)
            {
                List<TimesheetListDTO> recordsFiltered = new();

                recordsFiltered = _recordsByDateRange.Where(t => t.CompanyId == filter.CompanyId).ToList();

                _timesheetByDateRangeSum = recordsFiltered.Sum(t => t.TotalAmount);

                return await Task.FromResult(recordsFiltered);
            }
            else
            {
                _timesheetByDateRangeSum = _recordsByDateRange.Sum(t => t.TotalAmount);

                return await Task.FromResult(_recordsByDateRange);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<List<TimesheetCalendarDTO>> SetRecordsCalendarView(List<TimesheetListDTO> records)
    {
        try
        {
            List<TimesheetCalendarDTO> results = new();

            foreach (var record in records)
            {
                TimesheetCalendarDTO timesheetCalendarDTO = new();

                var result = results.Find(t => t.CompanyDescription == record.Description &&
                                               t.TDate.Date == record.TimeIn.Date);

                if (result is not null)
                {
                    result.TotalAmount += record.TotalAmount;
                }
                else
                {
                    timesheetCalendarDTO.TDate = record.TimeIn;
                    timesheetCalendarDTO.CompanyDescription = record.Description;
                    timesheetCalendarDTO.PayStatus = record.PayStatus;
                    timesheetCalendarDTO.TotalAmount = record.TotalAmount;
                    results.Add(timesheetCalendarDTO);
                }
            }

            return await Task.FromResult(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<List<TimesheetByCompanyGroupDTO>> SetRecordsListView(List<TimesheetListDTO> records)
    {
        try
        {
            var resultsByGroup = records.GroupBy(t => $"{t.Description}");
            var results = resultsByGroup.Select(tGroup => new TimesheetByCompanyGroupDTO()
            {
                Description = tGroup.Key,
                TotalAmount = tGroup.Sum(a => a.TotalAmount),
                Timesheets = tGroup.ToList()
            }).ToList();

            return await Task.FromResult(results);

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<TimesheetListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();

            _recordsByDateRange = await _timesheetData.GetRecordsByDateRange(user.Id, dateTimeRange);
            _timesheetByDateRangeSum = _recordsByDateRange.Sum(t => t.TotalAmount);

            return _recordsByDateRange;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }
}
