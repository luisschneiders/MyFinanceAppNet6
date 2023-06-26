using DateTimeLibrary.Models;
using MainApp.Settings.Enum;
using Microsoft.AspNetCore.Components;

namespace MainApp.Services;

public class AppSettingsService : IAppSettingsService
{
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
            string shape = await BuildShape(Radius.Round5);
            return await Task.FromResult(shape);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<string> GetCardShape()
    {
        try
        {
            string shape = await BuildShape(Radius.Round4);
            return await Task.FromResult(shape);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<string> GetMenuShape()
    {
        try
        {
            string shape = await BuildShape(Radius.Round5);
            return await Task.FromResult(shape);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<string> GetModalShape()
    {
        try
        {
            string shape = await BuildShape(Radius.Round4);
            return await Task.FromResult(shape);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<string> BuildShape(string radius)
    {
        try
        {
            string shape = await GetShapes();
            string componentRadius = string.Empty;

            switch (shape)
            {
                case "Default":
                    componentRadius = Radius.Default;
                    break;
                case "Square":
                    componentRadius = Radius.Square;
                    break;
                case "Round":
                    componentRadius = radius;
                    break;
            }

            return await Task.FromResult(componentRadius);

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
