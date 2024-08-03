namespace MainApp.Services;

public interface ITransactionCategoryService<T> : IBaseService<T>
{
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
}
