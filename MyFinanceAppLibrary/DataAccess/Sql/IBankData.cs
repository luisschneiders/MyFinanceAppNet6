namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankData<T> : IBaseData<T>
{
    Task<BankModelBalanceSumDTO> GetBankBalancesSum(string userId);
}
