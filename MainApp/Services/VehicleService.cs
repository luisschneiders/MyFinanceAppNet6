using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class VehicleService : IVehicleService<VehicleModel>
{
    [Inject]
    private IVehicleData<VehicleModel> _vehicleData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public VehicleService(IVehicleData<VehicleModel> vehicleData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _vehicleData = vehicleData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task ArchiveRecord(VehicleModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            VehicleModel recordModel = model;
            recordModel.IsArchived = true;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _vehicleData.ArchiveRecord(recordModel);
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
            var user = await GetLoggedInUser();
            VehicleModel recordModel = new()
            {
                Description = model.Description,
                Plate = model.Plate,
                UpdatedBy = user.Id
            };

            await _vehicleData.CreateRecord(recordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _vehicleData.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task<VehicleModel> GetRecordById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            VehicleModel result = await _vehicleData.GetRecordById(user.Id, modelId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    // TODO: Add pagination capabilities
    public async Task<List<VehicleModel>> GetRecords()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<VehicleModel> results = await _vehicleData.GetRecords(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<VehicleModel>> GetSearchResults(string search)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<VehicleModel> results = await _vehicleData.GetSearchResults(user.Id, search);
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
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();
            VehicleModel recordModel = new()
            {
                Id = model.Id,
                Description = model.Description,
                Plate = model.Plate,
                IsActive = model.IsActive,
                UpdatedBy = user.Id,
                UpdatedAt = DateTime.Now,
            };

            await _vehicleData.UpdateRecord(recordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(VehicleModel model)
    {
        // TODO: check if record is not archived in Mysql Stored Procedure
        try
        {
            var user = await GetLoggedInUser();

            VehicleModel recordModel = model;
            recordModel.IsActive = !model.IsActive;
            recordModel.UpdatedBy = user.Id;
            recordModel.UpdatedAt = DateTime.Now;

            await _vehicleData.UpdateRecordStatus(recordModel);
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

    public Task<List<VehicleModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }
}
