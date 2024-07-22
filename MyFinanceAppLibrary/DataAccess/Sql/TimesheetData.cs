namespace MyFinanceAppLibrary.DataAccess.Sql;

public class TimesheetData : ITimesheetData<TimesheetModel>
{
    private readonly IDataAccess _dataAccess;

    public TimesheetData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(TimesheetModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTimesheet_Archive",
                new
                {
                    timesheetId = model.Id,
                    timesheetIsArchived = model.IsArchived,
                    timesheetUpdatedBy = model.UpdatedBy,
                    timesheetUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
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
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTimesheet_Create",
                new
                {
                    timesheetCompanyId = model.CompanyId,
                    timesheetTimeIn = model.TimeIn,
                    timesheetTimeBreak = model.TimeBreak,
                    timesheetTimeOut = model.TimeOut,
                    timesheetPayStatus = model.PayStatus,
                    timesheetHoursWorked = model.HoursWorked,
                    timesheetOvertime = model.Overtime,
                    timesheetHourRate = model.HourRate,
                    timesheetTotalAmount = model.TotalAmount,
                    timesheetComments = model.Comments,
                    timesheetUpdatedBy = model.UpdatedBy,
                    timesheetCreatedAt = model.CreatedAt,
                    timesheetUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _dataAccess.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task<TimesheetModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<TimesheetModel, dynamic>(
                "myfinancedb.spTimesheet_GetById",
                new { userId, timesheetId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TimesheetModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TimesheetModel>> GetRecordsActive(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<TimesheetModel, dynamic>(
                "myfinancedb.spTimesheet_GetAllActive",
                new { userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TimesheetListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange)
    {
        try
        {
            var results = await _dataAccess.LoadData<TimesheetListDTO, dynamic>(
                "myfinancedb.spTimesheet_GetRecordsByDateRange",
                new
                {
                    userId,
                    startDate = dateTimeRange.Start,
                    endDate = dateTimeRange.End
                },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TimesheetModel>> GetSearchResults(string userId, string search)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRecord(TimesheetModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTimesheet_Update",
                new
                {
                    timesheetId = model.Id,
                    timesheetComments = model.Comments,
                    timesheetIsActive = model.IsActive,
                    timesheetUpdatedBy = model.UpdatedBy,
                    timesheetUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
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
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTimesheet_UpdateStatus",
                new
                {
                    timesheetId = model.Id,
                    timesheetIsActive = model.IsActive,
                    timesheetUpdatedBy = model.UpdatedBy,
                    timesheetUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
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
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTimesheet_UpdatePayStatus",
                new
                {
                    timesheetId = model.Id,
                    timesheetPayStatus = model.PayStatus,
                    timesheetUpdatedBy = model.UpdatedBy,
                    timesheetUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
