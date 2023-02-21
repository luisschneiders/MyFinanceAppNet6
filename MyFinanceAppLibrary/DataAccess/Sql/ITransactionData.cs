namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ITransactionData<T> : IBaseData<T>
{
    Task ArchiveRecordCredit(T model);
    Task ArchiveRecordDebit(T model);
    Task ArchiveRecordTransfer(T model);
    Task CreateRecordCredit(T model);
    Task CreateRecordDebit(T model);
    Task CreateRecordTransfer(T model);
    Task<List<TransactionModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRangeModel dateTimeRangeModel);
}
