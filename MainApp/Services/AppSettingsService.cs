using DateTimeLibrary.Models;
using MainApp.Settings.Enum;
using Microsoft.AspNetCore.Components;

namespace MainApp.Services;

public class AppSettingsService : IAppSettingsService
{
    //TODO: replace ILocalStorageService with IAppSettingsService
    [Inject]
    private ILocalStorageService _localStorageService { get; set; } = default!;

    public AppSettingsService(ILocalStorageService localStorageService)
	{
		_localStorageService = localStorageService;
	}

	public async Task SetShapes(string radius)
	{
		try
		{
            switch (radius)
            {
                case Radius.Default:
                    await _localStorageService.SetAsync<string>(LocalStorage.AppInterfaceShape, Shape.Default.ToString());
                    break;
                case Radius.Square:
                    await _localStorageService.SetAsync<string>(LocalStorage.AppInterfaceShape, Shape.Square.ToString());
                    break;
                case Radius.Round4:
                case Radius.Round5:
                    await _localStorageService.SetAsync<string>(LocalStorage.AppInterfaceShape, Shape.Round.ToString());
                    break;
            }

            await Task.CompletedTask;
        }
		catch (Exception ex)
		{
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
	}

    public async Task<string> GetShapes()
    {
        try
        {
            string shape = await _localStorageService.GetAsync<string>(LocalStorage.AppInterfaceShape);

            if (string.IsNullOrEmpty(shape))
            {
                shape = Radius.Default;
            }

            return await Task.FromResult(shape);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<string> GetButtonShape()
    {
        try
        {
            string shape = await GetShapes();
            string radius = string.Empty;

            switch (shape)
            {
                case "Default":
                    radius = Radius.Default;
                    break;
                case "Square":
                    radius = Radius.Square;
                    break;
                case "Round":
                    radius = Radius.Round5;
                    break;
            }

            return await Task.FromResult(radius);

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
