using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public class RandomNumberService : IRandomNumberService
{
    [Inject]
    ILocalStorageService _localStorageService { get; set; } = default!;

    public RandomNumberService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    public async Task<HashSet<int>> GetLocalStorageRandomNumber()
    {
        try
        {
            HashSet<int> numbers = await _localStorageService.GetAsync<HashSet<int>>(LocalStorage.AppToolRandomNumber);

            return await Task.FromResult(numbers);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task SetLocalStorageRandomNumber(HashSet<int> numbers)
    {
        try
        {
            await _localStorageService.SetAsync<HashSet<int>>(LocalStorage.AppToolRandomNumber, numbers);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
