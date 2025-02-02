using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class LocationExpenseService : ILocationExpenseService<LocationExpenseModel>
{
    [Inject]
    private ILocationExpenseData<LocationExpenseModel> _locationExpenseData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    public LocationExpenseService(
        ILocationExpenseData<LocationExpenseModel> locationExpenseData,
        IUserData userData,
        AuthenticationStateProvider authProvider)
    {
        _locationExpenseData = locationExpenseData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task<List<LocationExpenseDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange)
    {
        try
        {
            UserModel user = await GetLoggedInUser();
            List<LocationExpenseDTO> results = await _locationExpenseData.GetRecordsByDateRange(user.Id, dateTimeRange);
            return results;
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
