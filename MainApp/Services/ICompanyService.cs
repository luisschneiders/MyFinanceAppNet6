namespace MainApp.Services;

public interface ICompanyService<T> : IBaseService<T>
{
    public Task<decimal> GetHourRate(string modelId);
}
