namespace MainApp.Services;

public interface ICompanyService<T> : IBaseService<T>
{
    Task<decimal> GetHourRate(string modelId);
}
