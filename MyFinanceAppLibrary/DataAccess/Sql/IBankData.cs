namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankData<T> : IBaseData<T>
{
    public Task<BankBalanceSumDTO> GetBankBalancesSum(string userId);
}
