namespace MainApp.Services;

public interface IExpenseCategoryService<T> : IBaseService<T>
{
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
}
