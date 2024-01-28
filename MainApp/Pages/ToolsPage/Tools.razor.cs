using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.ToolsPage;

public partial class Tools : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private int _randonNumberMin { get; set; } = 1;
    private int _randonNumberMax { get; set; } = 1000;
    // private int _randomNumberResult { get; set; } = 0;
    private List<int> _randomNumbers { get; set; } = new();
    private bool _isProcessing { get; set; } = false;

    public Tools()
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
