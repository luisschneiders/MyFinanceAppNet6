namespace MainApp.Services;

public interface IBankService<T> : IBaseService<T>
{
    public Task<BankBalanceSumDTO> GetBankBalancesSum();
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
}
