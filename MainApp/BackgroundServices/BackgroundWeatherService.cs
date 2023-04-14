using Google.Protobuf.WellKnownTypes;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;

namespace MainApp.BackgroundServices;

public class BackgroundWeatherService : BackgroundService
{
    private readonly ILogger<BackgroundWeatherService> _logger;
    private readonly IServiceScopeFactory<IDateTimeService> _serviceScopeFactory;

    public BackgroundWeatherService(ILogger<BackgroundWeatherService> logger, IServiceScopeFactory<IDateTimeService> serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                //var dateTimeService = _serviceScopeFactory.Get();
                //var currentTime = dateTimeService.GetCurrentTime();
                // TODO: Get weather condition every hour
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Job {jobName} threw an exception", nameof(BackgroundWeatherService));
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
