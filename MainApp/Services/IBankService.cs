namespace MainApp.Services;

public interface IBankService
{
    Task<List<BankModel>> GetBanks();
    Task<BankModel> GetBankById(string bankId);
    Task<ulong> GetLastInsertedId();
    Task CreateBank(BankModel bankModel);
    Task UpdateBank(BankModel bankModel);
}
