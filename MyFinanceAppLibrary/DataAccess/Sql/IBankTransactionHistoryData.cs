namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBankTransactionHistoryData<T> : IBaseData<T>
{
    public Task<List<BankTransactionHistoryListDTO>> GetRecordsByBankLoadMore(string userId, ulong bankId, ulong? lastId, LoadMore loadMore);
}
