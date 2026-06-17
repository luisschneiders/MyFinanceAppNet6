namespace MainApp.Services;

public interface ICompanyService<T> : IBaseService<T>
{
    public Task<List<CheckboxItemModel>> GetRecordsForFilter();
    public Task<decimal> GetHourRate(string modelId);
    public Task<decimal> GetCPK(string modelId);
}
