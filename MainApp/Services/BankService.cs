using Microsoft.AspNetCore.Components;

namespace MainApp.Services;

public class BankService : IBankService
{
    [Inject]
    IBankData _bankData { get; set; } = default!;

    public BankService(IBankData bankData)
    {
        _bankData = bankData;
    }

    public async Task<List<BankModel>> GetAllBanksByUserId(string userId)
    {
        List<BankModel> results = await _bankData.GetAllBanksByUserId(userId);
        return results;
    }
}
