namespace MainApp.Factories;

public class ServiceScopeFactory<T> : IServiceScopeFactory<T> where T : class
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public T Get() => _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<T>();
}
