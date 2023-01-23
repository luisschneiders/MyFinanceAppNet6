namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankData
{
    Task<List<BankModel>> GetAllBanksByUserId(string userId);
}
