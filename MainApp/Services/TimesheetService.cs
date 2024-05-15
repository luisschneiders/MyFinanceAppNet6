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
    private decimal _timesheetTotalAmountSum { get; set; } = 0;
    private TimeSpan _timesheetTotalWorkHoursSum { get; set; }
    private TimeSpan _timesheetTotalOvertimeSum { get; set; }

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

    public async Task<List<TimesheetListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();

            _recordsByDateRange = await _timesheetData.GetRecordsByDateRange(user.Id, dateTimeRange);
            _timesheetTotalAmountSum = _recordsByDateRange.Sum(t => t.TotalAmount);

            return _recordsByDateRange;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TimesheetByCompanyGroupDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, FilterTimesheetDTO filterTimesheetDTO)
    {
        try
        {
            if (filterTimesheetDTO.CompanyId > 0)
            {
                List<TimesheetListDTO> recordsFiltered = new();

                recordsFiltered = _recordsByDateRange.Where(t => t.CompanyId == filterTimesheetDTO.CompanyId).ToList();

                var results = SetRecordsByGroup(recordsFiltered);

                _timesheetTotalAmountSum = recordsFiltered.Sum(t => t.TotalAmount);

                return await Task.FromResult(results);
            }
            else
            {
                var results = SetRecordsByGroup(_recordsByDateRange);

                _timesheetTotalAmountSum = _recordsByDateRange.Sum(t => t.TotalAmount);

                return await Task.FromResult(results);
            }
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
            return await Task.FromResult(_timesheetTotalAmountSum);
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

    public async Task<List<TimesheetByCompanyGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRange dateTimeRange)
    {
        try
        {
            var records = await GetRecordsByDateRange(dateTimeRange);
            var results = SetRecordsByGroup(records);
            _timesheetTotalAmountSum = records.Sum(t => t.TotalAmount);

            return results;
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

    private static List<TimesheetByCompanyGroupDTO> SetRecordsByGroup(List<TimesheetListDTO> records)
    {
        var resultsByGroup = records.GroupBy(t => $"{t.Description}");
        var results = resultsByGroup.Select(tGroup => new TimesheetByCompanyGroupDTO()
        {
            Description = tGroup.Key,
            TotalAmount = tGroup.Sum(a => a.TotalAmount),
            Timesheets = tGroup.ToList()
        }).ToList();

        return results;
    }
}
