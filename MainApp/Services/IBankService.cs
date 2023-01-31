namespace MainApp.Services;

public interface IBankService
{
    Task<List<BankModel>> GetBanks();
    Task<List<BankModel>> GetSearchResults(string search);
    Task<BankModel> GetBankById(string modelId);
    Task<ulong> GetLastInsertedId();
    Task CreateBank(BankModel model);
    Task UpdateBank(BankModel model);
    Task UpdateBankStatus(BankModel model);
    Task ArchiveBank(BankModel model);
}
