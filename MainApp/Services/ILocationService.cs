namespace MainApp.Services;

public interface ILocationService<T>
{
    Task SaveRecord(T model);
}
