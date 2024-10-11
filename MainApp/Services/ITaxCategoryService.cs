namespace MainApp.Services;

public interface ITaxCategoryService<T> : IBaseService<T>
{
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
}
