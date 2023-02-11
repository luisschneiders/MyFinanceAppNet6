using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.Sql;

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

            TimesheetModel recordModel = model;
            recordModel.IsArchived = true;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _timesheetData.ArchiveRecord(recordModel);
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

            TimesheetModel recordModel = new()
            {
                CompanyId = model.CompanyId,
                TimeIn = model.TimeIn,
                TimeBreak = model.TimeBreak,
                TimeOut = model.TimeOut,
                Comments = model.Comments,
                HourRate = model.HourRate,
                UpdatedBy = user.Id
            };

            await _timesheetData.CreateRecord(recordModel);
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

    public async Task<List<TimesheetModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TimesheetModelListDTO> results = await _timesheetData.GetRecordsByDateRange(user.Id, dateTimeRangeModel);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
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

    public Task<List<TimesheetModel>> GetSearchResults(string search)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRecord(TimesheetModel model)
    {
        try
        {
            var user = await GetLoggedInUser();
            TimesheetModel recordModel = new()
            {
                Id = model.Id,
                Comments = model.Comments,
                IsActive = model.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _timesheetData.UpdateRecord(recordModel);
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

            TimesheetModel recordModel = model;
            recordModel.IsActive = !model.IsActive;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _timesheetData.UpdateRecordStatus(recordModel);
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

            TimesheetModel recordModel = model;
            recordModel.PayStatus = model.PayStatus;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _timesheetData.UpdateRecordPayStatus(recordModel);
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
