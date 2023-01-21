using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess;

namespace MainApp.Pages;

public partial class Index : ComponentBase
{
#nullable disable
    [Inject]
    IUserData userData { get; set; }

    [Inject]
    AuthenticationStateProvider authProvider { get; set; }
#nullable enable

    private UserModel _loggedInUser { get; set; } = new();

    protected async override Task OnInitializedAsync()
    {
        await LoadAndVerifyUser();
    }

    private async Task LoadAndVerifyUser()
    {
        var authState = await authProvider.GetAuthenticationStateAsync();
        string? objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;

        if (string.IsNullOrWhiteSpace(objectId) == false)
        {
            _loggedInUser = await userData.GetUserFromAuthentication(objectId) ?? new();
        }

        string? firstName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("givenname"))?.Value;
        string? lastName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("surname"))?.Value;
        string? displayName = authState.User.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
        string? email = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value;

        bool isDirty = false;

        if (objectId?.Equals(_loggedInUser.ObjectIdentifier) == false)
        {
            isDirty = true;
            _loggedInUser.ObjectIdentifier = objectId;
        }

        if (firstName?.Equals(_loggedInUser.FirstName) == false)
        {
            isDirty = true;
            _loggedInUser.FirstName = firstName;
        }

        if (lastName?.Equals(_loggedInUser.LastName) == false)
        {
            isDirty = true;
            _loggedInUser.LastName = lastName;
        }

        if (displayName?.Equals(_loggedInUser.DisplayName) == false)
        {
            isDirty = true;
            _loggedInUser.DisplayName = displayName;
        }

        if (email?.Equals(_loggedInUser.EmailAddress) == false)
        {
            isDirty = true;
            _loggedInUser.EmailAddress = email;
        }

        if (isDirty)
        {
            if (string.IsNullOrWhiteSpace(_loggedInUser.Id))
            {
                await userData.CreateUser(_loggedInUser);
            }
            else
            {
                await userData.UpdateUser(_loggedInUser);
            }
        }

    }
}
