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

    private List<TimesheetListDTO> _resultListByDateRange { get; set; } = new();

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
            var results = await _timesheetData.GetRecordsByDateRange(user.Id, dateTimeRange);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TimesheetListDTO>> GetRecordsByFilter(DateTimeRange dateTimeRange, CompanyModel companyModel)
    {
        try
        {
            var records = await GetRecordsByDateRange(dateTimeRange);

            if (companyModel.Id > 0)
            {
                return _resultListByDateRange = records.Where(c => c.CompanyId == companyModel.Id).ToList();
            }
            else
            {
                return _resultListByDateRange = records;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<decimal> GetSumTotalAwaiting()
    {
        var totalRecords = _resultListByDateRange.Where(p => p.PayStatus == (int)PayStatus.Awaiting);
        var totalSum = totalRecords.Sum(s => s.TotalAmount);
        return await Task.FromResult(totalSum);
    }

    public async Task<decimal> GetSumTotalPaid()
    {
        var totalRecords = _resultListByDateRange.Where(p => p.PayStatus == (int)PayStatus.Paid);
        var totalSum = totalRecords.Sum(s => s.TotalAmount);
        return await Task.FromResult(totalSum);
    }

    public async Task<double> GetSumTotalHours()
    {
        var totalHours = _resultListByDateRange.Sum(s => s.HoursWorked.TotalHours);
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

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }
}
