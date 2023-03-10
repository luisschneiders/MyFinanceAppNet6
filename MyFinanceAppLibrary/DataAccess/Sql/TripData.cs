namespace MyFinanceAppLibrary.DataAccess.Sql;

public class TripData : ITripData<TripModel>
{
    private readonly IDataAccess _dataAccess;

    public TripData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(TripModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spTrip_Archive",
                new
                {
                    tripId = model.Id,
                    tripIsActive = model.IsActive,
                    tripIsArchived = model.IsArchived,
                    tripUpdatedBy = model.UpdatedBy,
                    tripUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(TripModel model)
    {
        try
        {
            await _dataAccess.LoadData<TripModel, dynamic>(
                "myfinancedb.spTrip_Create",
                new
                {
                    tripTDate = model.TDate,
                    tripVehicleId = model.VehicleId,
                    tripDistance = model.Distance,
                    tripUpdatedBy = model.UpdatedBy,
                    tripCreatedAt = model.CreatedAt,
                    tripUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public async Task<TripModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<TripModel, dynamic>(
                "myfinancedb.spTrip_GetById",
                new { userId = userId, tripId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TripModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TripModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TripListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange)
    {
        try
        {
            var results = await _dataAccess.LoadData<TripListDTO, dynamic>(
                "myfinancedb.spTrip_GetRecordsByDateRange",
                new
                {
                    userId = userId,
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

    public Task<List<TripModel>> GetSearchResults(string userId, string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(TripModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(TripModel model)
    {
        throw new NotImplementedException();
    }
}
