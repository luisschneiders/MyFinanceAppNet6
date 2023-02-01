namespace MainApp.Services;

public interface ITransactionCategoryService
{
    Task<List<TransactionCategoryModel>> GetTransactionCategories();
    Task<List<TransactionCategoryModel>> GetSearchResults(string search);
    Task<TransactionCategoryModel> GetTransactionCategoryById(string modelId);
    Task<ulong> GetLastInsertedId();
    Task CreateTransactionCategory(TransactionCategoryModel model);
    Task UpdateTransactionCategory(TransactionCategoryModel model);
    Task UpdateTransactionCategoryStatus(TransactionCategoryModel model);
    Task ArchiveTransactionCategory(TransactionCategoryModel model);
}
