namespace MainApp.Services;

public interface ITransactionTypeService
{
    Task<List<TransactionTypeModel>> GetTransactionTypes();
    Task<List<TransactionTypeModel>> GetSearchResults(string search);
    Task<TransactionTypeModel> GetTransactionTypeById(string modelId);
    Task<ulong> GetLastInsertedId();
    Task CreateTransactionType(TransactionTypeModel model);
    Task UpdateTransactionType(TransactionTypeModel model);
    Task UpdateTransactionTypeStatus(TransactionTypeModel model);
    Task ArchiveTransactionType(TransactionTypeModel model);
}
