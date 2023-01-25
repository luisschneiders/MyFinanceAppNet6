namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankData
{
    Task<List<BankModel>> GetBanks(string userId);
    Task<BankModel> GetBankById(string userId, string bankId);
    Task<ulong> GetLastInsertedId();
    Task CreateBank(BankModel bankModel);
    Task UpdateBank(BankModel bankModel);
}
