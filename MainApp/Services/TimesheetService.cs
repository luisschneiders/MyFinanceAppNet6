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
    
    [Inject]
    ICompanyService<CompanyModel> _companyService { get; set; } = default!;

    private List<TimesheetListDTO> _recordsByDateRange { get; set; } = new();
    private TimesheetTotal _timesheetTotal { get; set; } = new();

    public TimesheetService(ITimesheetData<TimesheetModel> timesheetData,
                            IUserData userData,
                            AuthenticationStateProvider authProvider,
                            ICompanyService<CompanyModel> companyService)
    {
        _timesheetData = timesheetData;
        _userData = userData;
        _authProvider = authProvider;
        _companyService = companyService;
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
            List<CompanyModel> companies = await _companyService.GetRecords();

            if (model.HoursWorked != TimeSpan.FromHours(companies.First(c => c.Id == model.CompanyId).StandardHours) &&
                model.HoursWorked > TimeSpan.FromHours(companies.First(c => c.Id == model.CompanyId).StandardHours))
            {
                model.Overtime = model.HoursWorked - TimeSpan.FromHours(companies.First(c => c.Id == model.CompanyId).StandardHours);
            }

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

    public async Task<List<TimesheetByCompanyGroupDTO>> GetRecordsListView(MultiFilterTimesheetDTO filter)
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

    public async Task<List<TimesheetCalendarDTO>> GetRecordsCalendarView(MultiFilterTimesheetDTO filter)
    {
        try
        {
            List<TimesheetListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<TimesheetListDTO> recordsFiltered = new();
            List<TimesheetCalendarDTO> calendarData = new();
            
            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                _timesheetTotal = new(recordsFiltered.Sum(t => t.TotalAmount), recordsFiltered.Sum(t => t.HoursWorked.TotalHours));
                calendarData = await SetRecordsCalendarView(recordsFiltered);
            }
            else
            {
                _timesheetTotal = new(records.Sum(t => t.TotalAmount), records.Sum(t => t.HoursWorked.TotalHours));
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

    public async Task<TimesheetTotal> GetTotals()
    {
        try
        {
            return await Task.FromResult(_timesheetTotal);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
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

    public async Task<List<CheckboxItemModel>> GetRecordsForFilter()
    {
        try
        {

            PayStatus[] results = (PayStatus[])Enum.GetValues(typeof(PayStatus));

            List<CheckboxItemModel> filter = new();

            foreach (var (status, index) in results.Select((value, index) => (value, index)))
            {
                ulong statusValue = (ulong)index;
                CheckboxItemModel filterItem = new()
                {
                    Id = statusValue,
                    Description = status.ToString(),
                };
                filter.Add(filterItem);
            }

            return await Task.FromResult(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<TimesheetListDTO>> SetRecordsFilter(MultiFilterTimesheetDTO filter)
    {
        try
        {
            if (filter.CompanyId.Count > 0 || filter.StatusId.Count > 0)
            {
                List<TimesheetListDTO> recordsFiltered = new();

                if (filter.CompanyId.Count > 0 && filter.StatusId.Count == 0) // Filter by Company only
                {
                    recordsFiltered = _recordsByDateRange.Where(t => filter.CompanyId.Contains(t.CompanyId)).ToList();
                }
                else if (filter.CompanyId.Count == 0 && filter.StatusId.Count > 0) // Filter by Status only
                {
                    recordsFiltered = _recordsByDateRange.Where(t => filter.StatusId.Contains((ulong)t.PayStatus)).ToList();
                }
                else // Filter by Company and Status
                {
                    recordsFiltered = _recordsByDateRange.Where(t => filter.CompanyId.Contains(t.CompanyId) && 
                                                         filter.StatusId.Contains((ulong)t.PayStatus)).ToList();
                }

                _timesheetTotal = new(recordsFiltered.Sum(t => t.TotalAmount), recordsFiltered.Sum(t => t.HoursWorked.TotalHours));

                return await Task.FromResult(recordsFiltered);
            }
            else
            {
                _timesheetTotal = new(_recordsByDateRange.Sum(t => t.TotalAmount), _recordsByDateRange.Sum(t => t.HoursWorked.TotalHours));

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
                TotalWorkHours = tGroup.Sum(h => h.HoursWorked.TotalHours),
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
            _timesheetTotal = new(_recordsByDateRange.Sum(t => t.TotalAmount), _recordsByDateRange.Sum(t => t.HoursWorked.TotalHours));

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
        return await _authProvider.GetUserFromAuth(_userData);
    }
}
