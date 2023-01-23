namespace MainApp.Services;

public interface IBankService
{
    Task<List<BankModel>> GetAllBanksByUserId();
}
