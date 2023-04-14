namespace MainApp.Factories;

public interface IServiceScopeFactory<T> where T : class
{
    T Get();
}
