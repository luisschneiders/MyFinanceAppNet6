using Microsoft.AspNetCore.Components;
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

    private List<TripListDTO> _recordsByDateRange { get; set; } = new();
    private decimal _tripDistanceByDateRangeSum { get; set; } = 0;

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

    public async Task<TripModel> GetRecordById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            TripModel result = await _tripData.GetRecordById(user.Id, modelId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<TripModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public Task<List<TripModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }

    public async Task<List<TripByVehicleGroupDTO>> GetRecordsListView(MultiFilterTripDTO filter)
    {
        try
        {
            List<TripListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<TripListDTO> recordsFiltered = new();
            List<TripByVehicleGroupDTO> results = new();

            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                results = await SetRecordsListView(recordsFiltered);
            }
            else
            {
                results = await SetRecordsListView(records);
            }

            return await Task.FromResult(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<decimal> GetSumByDateRange()
    {
        try
        {
            return await Task.FromResult(_tripDistanceByDateRangeSum);
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

    public async Task UpdateRecordPayStatus(TripModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _tripData.UpdateRecordPayStatus(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordTripCategory(TripModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _tripData.UpdateRecordTripCategory(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<List<TripByVehicleGroupDTO>> SetRecordsListView(List<TripListDTO> records)
    {
        try
        {
            var resultsByGroup = records.GroupBy(t => $"{t.VehicleDescription} - {t.VehiclePlate} ({t.VehicleYear})");
            var results = resultsByGroup.Select(tGroup => new TripByVehicleGroupDTO()
            {
                Description = tGroup.Key,
                Total = tGroup.Sum(a => a.Distance),
                Trips = tGroup.ToList()
            }).ToList();

            return await Task.FromResult(results);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<TripListDTO>> SetRecordsFilter(MultiFilterTripDTO filter)
    {
        try
        {
            if (filter.VehicleId.Count > 0 || filter.TCategoryId.Count > 0)
            {
                List<TripListDTO> recordsFiltered = new();

                if (filter.VehicleId.Count > 0 && filter.TCategoryId.Count == 0) // Filter by Vehicle only
                {
                    recordsFiltered = _recordsByDateRange.Where(t => filter.VehicleId.Contains(t.VehicleId)).ToList();
                }
                else if (filter.VehicleId.Count == 0 && filter.TCategoryId.Count > 0) // Filter by Trip only
                {
                    recordsFiltered = _recordsByDateRange.Where(t => filter.TCategoryId.Contains(t.TCategoryId)).ToList();
                }
                else // Filter by Vehicle and Trip Category
                {
                    recordsFiltered = _recordsByDateRange.Where(t => filter.VehicleId.Contains(t.VehicleId) && 
                                                         filter.TCategoryId.Contains(t.TCategoryId)).ToList();
                }

                _tripDistanceByDateRangeSum = recordsFiltered.Sum(t => t.Distance);

                return await Task.FromResult(recordsFiltered);
            }
            else
            {
                _tripDistanceByDateRangeSum = _recordsByDateRange.Sum(t => t.Distance);

                return await Task.FromResult(_recordsByDateRange);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<TripListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();

            _recordsByDateRange = await _tripData.GetRecordsByDateRange(user.Id, dateTimeRange);
            _tripDistanceByDateRangeSum = _recordsByDateRange.Sum(t => t.Distance);

            return _recordsByDateRange;
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
