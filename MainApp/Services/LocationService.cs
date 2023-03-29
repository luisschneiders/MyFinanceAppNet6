using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.Sql;

namespace MainApp.Services;

public class LocationService : ILocationService<UserLocationModel>
{
    [Inject]
    private ILocationData<UserLocationModel> _locationData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public LocationService(ILocationData<UserLocationModel> locationData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _locationData = locationData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task SaveRecord(UserLocationModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _locationData.SaveRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<LocationModel> GetRecordById()
    {
        try
        {
            var user = await GetLoggedInUser();
            LocationModel result = await _locationData.GetRecordById(user.Id);
            return result;
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
