namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankData
{
    Task<List<BankModel>> GetAllBanks(string userId);
    Task<BankModel> GetBankById(string userId, string bankId);
}
