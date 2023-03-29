namespace MyFinanceAppLibrary.DataAccess.Sql;

public class LocationData : ILocationData<UserLocationModel>
{
    private readonly IDataAccess _dataAccess;

    public LocationData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<LocationModel> GetRecordById(string userId)
    {
        try
        {
            var result = await _dataAccess.LoadData<LocationModel, dynamic>(
                "myfinancedb.spLocation_GetById",
                new { userId = userId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task SaveRecord(UserLocationModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spLocation_Save",
                new
                {
                    locationAddress = model.Location.Address,
                    locationLatitude = model.Location.Latitude,
                    locationLongitude = model.Location.Longitude,
                    locationUpdatedBy = model.UpdatedBy,
                    locationCreatedAt = model.CreatedAt,
                    locationUpdatedAt = model.UpdatedAt,
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
