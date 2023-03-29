namespace MainApp.Services;

public interface ILocationService<T>
{
    Task<LocationModel> GetRecordById();
    Task SaveRecord(T model);
}
