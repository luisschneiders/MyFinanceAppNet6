using Google.Protobuf.WellKnownTypes;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;

namespace MainApp.BackgroundServices;

public class BackgroundWeatherService : BackgroundService
{
    private readonly IServiceScopeFactory<IDateTimeService> _serviceScopeFactory;

    public BackgroundWeatherService(IServiceScopeFactory<IDateTimeService> serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var dateTimeService = _serviceScopeFactory.Get();

                await Task.Delay(5000, stoppingToken);

                Console.WriteLine("LFS - current time is: " + dateTimeService.GetCurrentTime());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
