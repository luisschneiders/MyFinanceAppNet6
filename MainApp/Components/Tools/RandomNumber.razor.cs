using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class RandomNumber : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private int _randonNumberMin { get; set; } = 1;
    private int _randonNumberMax { get; set; } = 1000;
    private List<int> _randomNumbers { get; set; } = new();
    private bool _isProcessing { get; set; } = false;

    public RandomNumber()
    {
    }

    private async Task GenerateAsync()
    {
        _isProcessing = true;

        Random random = new Random();

        await Task.Delay(500);

        int randomNumberResult = random.Next(_randonNumberMin, _randonNumberMax + 1);
        _randomNumbers.Add(randomNumberResult);

        _isProcessing = false;

        await Task.CompletedTask;
    }
}
