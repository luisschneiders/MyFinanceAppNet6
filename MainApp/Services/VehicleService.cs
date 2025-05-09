﻿using Microsoft.AspNetCore.Components;
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
            UserModel user = await GetLoggedInUser();

            model.IsArchived = true;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _vehicleData.ArchiveRecord(model);
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
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _vehicleData.CreateRecord(model);
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
            UserModel user = await GetLoggedInUser();
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
            UserModel user = await GetLoggedInUser();
            List<VehicleModel> results = await _vehicleData.GetRecords(user.Id);
            return results;
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
            UserModel user = await GetLoggedInUser();
            List<VehicleModel> results = await _vehicleData.GetRecords(user.Id);

            List<CheckboxItemModel> filter = new();

            foreach (var item in results)
            {
                CheckboxItemModel filterItem = new()
                {
                    Id = item.Id,
                    Description = item.Description,
                };
                filter.Add(filterItem);
            }

            return filter;
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
            UserModel user = await GetLoggedInUser();
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
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _vehicleData.UpdateRecord(model);
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
            UserModel user = await GetLoggedInUser();

            model.IsActive = !model.IsActive;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _vehicleData.UpdateRecordStatus(model);
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

    public async Task<List<VehicleModel>> GetRecordsActive()
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<VehicleModel> results = await _vehicleData.GetRecordsActive(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
