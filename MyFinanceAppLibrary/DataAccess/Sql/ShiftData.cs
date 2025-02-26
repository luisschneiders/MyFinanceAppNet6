namespace MyFinanceAppLibrary.DataAccess.Sql;

public class ShiftData : IShiftData<ShiftModel>
{
    private readonly IDataAccess _dataAccess;
    public ShiftData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(ShiftModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spShift_Archive",
                new
                {
                    shiftId = model.Id,
                    shiftIsArchived = model.IsArchived,
                    shiftUpdatedBy = model.UpdatedBy,
                    shiftUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task CreateRecord(ShiftModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public Task<ShiftModel> GetRecordById(string userId, string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftModel>> GetSearchResults(string userId, string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(ShiftModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(ShiftModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ShiftListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange)
    {
        try
        {
            var results = await _dataAccess.LoadData<ShiftListDTO, dynamic>(
                "myfinancedb.spShift_GetRecordsByDateRange",
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

    public async Task SaveRecord(ShiftModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spShift_Save",
                new
                {
                    shiftSDate = model.SDate,
                    shiftCompanyId = model.CompanyId,
                    shiftIsAvailable = model.IsAvailable,
                    shiftUpdatedBy = model.UpdatedBy,
                    shiftCreatedAt = model.CreatedAt,
                    shiftUpdatedAt = model.UpdatedAt,
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
