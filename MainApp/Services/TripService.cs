﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class TripService : ITripService<TripModel>
{
    [Inject]
    private ITripData<TripModel> _tripData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public TripService(
        ITripData<TripModel> tripData,
        IUserData userData,
        AuthenticationStateProvider authProvider)
    {
        _tripData = tripData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task ArchiveRecord(TripModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.IsArchived = true;
            model.IsActive = false;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _tripData.ArchiveRecord(model);
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
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _tripData.CreateRecord(model);
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

    public Task<TripModel> GetRecordById(string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TripModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public Task<List<TripModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }

    public async Task<List<TripModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TripModelListDTO> results = await _tripData.GetRecordsByDateRange(user.Id, dateTimeRangeModel);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TripModel>> GetSearchResults(string search)
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

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }
}