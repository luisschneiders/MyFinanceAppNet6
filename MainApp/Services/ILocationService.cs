namespace MainApp.Services;

public interface ILocationService<T>
{
    public Task<LocationModel> GetRecordById();
    public Task SaveRecord(T model);
}
