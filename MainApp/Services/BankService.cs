﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class BankService : IBankService
{
    [Inject]
    IBankData _bankData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public BankService(IBankData bankData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _bankData = bankData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task<List<BankModel>> GetAllBanks()
    {
        var user = await GetLoggedInUser();
        List<BankModel> results = await _bankData.GetAllBanks(user.Id);

        return results;
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser =  await _authProvider.GetUserFromAuth(_userData);
    }

    public async Task<BankModel> GetBankById(string bankId)
    {
        var user = await GetLoggedInUser();
        BankModel result = await _bankData.GetBankById(user.Id, bankId);

        return result;
    }
}
