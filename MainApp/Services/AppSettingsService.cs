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

    public async Task SetTableColumns(string view, List<TableColumn> columns)
    {
        try
        {
            switch (view)
            {
                case PageView.Timesheet:
                    await _localStorageService.SetAsync<List<TableColumn>>(LocalStorage.AppTimesheetTableColumn, columns);
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

    public async Task<AppSettings> GetInterface()
    {
        try
        {
            AppSettings appSettings = new()
            {
                Button = await GetButtonShape(),
                Card = await GetCardShape(),
                Form = await GetFormShape(),
                Menu = await GetMenuShape(),
                Modal = await GetModalShape()
            };

            return await Task.FromResult(appSettings);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<string> GetLocalStorageStartOfWeek()
    {
        try
        {
            string? localStorage = await _localStorageService.GetAsync<string>(LocalStorage.AppStartOfWeek);

            return await Task.FromResult(localStorage!);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task SetLocalStorageStartOfWeek(string startOfWeek)
    {
        try
        {
            await _localStorageService.SetAsync<string>(LocalStorage.AppStartOfWeek, startOfWeek);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<string> GetLocalStorageAppTheme()
    {
        try
        {
            string? localStorage = await _localStorageService.GetAsync<string>(LocalStorage.AppTheme);

            return await Task.FromResult(localStorage!);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task SetLocalStorageAppTheme(string theme)
    {
        try
        {
            await _localStorageService.SetAsync<string>(LocalStorage.AppTheme, theme);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<string> GetButtonShape()
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

    private async Task<string> GetCardShape()
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

    private async Task<string> GetFormShape()
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

    private async Task<string> GetMenuShape()
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

    private async Task<string> GetModalShape()
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
