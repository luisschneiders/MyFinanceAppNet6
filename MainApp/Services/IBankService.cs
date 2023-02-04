namespace MainApp.Services;

public interface IBankService<T> : IBaseService<T>
{
    Task<BankModelBalanceSumDTO> GetBankBalancesSum();
}
