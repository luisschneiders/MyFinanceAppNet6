namespace MainApp.Services;

public interface IBankService
{
    Task<List<BankModel>> GetBanks();
    Task<BankModel> GetBankById(string bankId);
    Task CreateBank(BankModel bankModel);
    Task<ulong> GetLastInsertedId();
}
