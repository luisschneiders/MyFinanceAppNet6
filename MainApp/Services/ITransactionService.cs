namespace MainApp.Services;

public interface ITransactionService<T> : IBaseService<T>
{
    Task CreateRecordCredit(T model);
    Task CreateRecordDebit(T model);
    Task CreateRecordTransfer(T model);
    Task<List<TransactionModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel);
    Task<List<TransactionModelByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRangeModel dateTimeRangeModel);
}
