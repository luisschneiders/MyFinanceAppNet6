namespace MyFinanceAppLibrary.DataAccess.Sql;

public class VehicleData : IVehicleData<VehicleModel>
{
    private readonly IDataAccess _dataAccess;

    public VehicleData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(VehicleModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spVehicle_Archive",
                new
                {
                    vehicleId = model.Id,
                    vehicleIsArchived = model.IsArchived,
                    vehicleUpdatedBy = model.UpdatedBy,
                    vehicleUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(VehicleModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spVehicle_Create",
                new
                {
                    vehicleDescription = model.Description,
                    vehiclePlate = model.Plate,
                    vehicleUpdatedBy = model.UpdatedBy,
                    vehicleCreatedAt = model.CreatedAt,
                    vehicleUpdatedAt = model.UpdatedAt,
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

    public async Task<VehicleModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<VehicleModel, dynamic>(
                "myfinancedb.spVehicle_GetById",
                new { userId = userId, vehicleId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<VehicleModel>> GetRecords(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<VehicleModel, dynamic>(
                "myfinancedb.spVehicle_GetAll",
                new { userId = userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<VehicleModel>> GetRecordsActive(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<VehicleModel, dynamic>(
                "myfinancedb.spVehicle_GetAllActive",
                new { userId = userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<VehicleModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<VehicleModel, dynamic>(
                "myfinancedb.spVehicle_GetSearchResults",
                new
                {
                    userId = userId,
                    searchVehicle = search,
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

    public async Task UpdateRecord(VehicleModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spVehicle_Update",
                new
                {
                    vehicleId = model.Id,
                    vehicleDescription = model.Description,
                    vehiclePlate = model.Plate,
                    vehicleIsActive = model.IsActive,
                    vehicleUpdatedBy = model.UpdatedBy,
                    vehicleUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(VehicleModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spVehicle_UpdateStatus",
                new
                {
                    vehicleId = model.Id,
                    vehicleIsActive = model.IsActive,
                    vehicleUpdatedBy = model.UpdatedBy,
                    vehicleUpdatedAt = model.UpdatedAt,
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
