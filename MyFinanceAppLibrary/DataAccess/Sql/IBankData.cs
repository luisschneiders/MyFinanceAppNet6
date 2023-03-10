namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankData<T> : IBaseData<T>
{
    Task<BankBalanceSumDTO> GetBankBalancesSum(string userId);
}
