namespace MainApp.Services;

public interface IBankTransactionHistoryService<T> : IBaseService<T>
{
    public Task<List<BankTransactionHistoryByDateGroupDTO>> GetRecordsListView(MultiFilterBankTransactionHistoryDTO filter);
}
