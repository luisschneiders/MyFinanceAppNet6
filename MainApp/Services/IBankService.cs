namespace MainApp.Services;

public interface IBankService
{
    Task<List<BankModel>> GetAllBanks();
    Task<BankModel> GetBankById(string bankId);
}
