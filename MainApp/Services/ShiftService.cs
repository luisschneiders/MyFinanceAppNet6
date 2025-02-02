using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class ShiftService : IShiftService<ShiftModel>
{
    [Inject]
    private IShiftData<ShiftModel> _shiftData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private List<ShiftListDTO> _recordsByDateRange { get; set; } = new();

    public ShiftService(IShiftData<ShiftModel> shiftData,
                        IUserData userData,
                        AuthenticationStateProvider authProvider)
    {
        _shiftData = shiftData;
        _userData = userData;
        _authProvider = authProvider;

    }
    public Task ArchiveRecord(ShiftModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateRecord(ShiftModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public Task<ShiftModel> GetRecordById(string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }

    public Task<List<ShiftModel>> GetSearchResults(string search)
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

    public async Task SaveRecord(ShiftModel model)
    {
        try
        {
            UserModel user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _shiftData.SaveRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ShiftListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange)
    {
                try
        {
            UserModel user = await GetLoggedInUser();

            _recordsByDateRange = await _shiftData.GetRecordsByDateRange(user.Id, dateTimeRange);

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
        return await _authProvider.GetUserFromAuth(_userData);
    }
}
