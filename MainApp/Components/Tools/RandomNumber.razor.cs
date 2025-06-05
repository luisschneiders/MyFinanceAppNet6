using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class RandomNumber : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private int _maxNumber { get; set; } = 44;
    private HashSet<int> _randomNumbers { get; set; } = new();
    private HashSet<string> _uniqueSets { get; set; } = new();
    private bool _isProcessing { get; set; } = false;

    public RandomNumber()
    {
    }

    private async Task AddNumberAsync(int number)
    {
        if (!_randomNumbers.Add(number))
        {
            _randomNumbers.Remove(number); // Toggle off
        }

        await Task.CompletedTask;
    }

    private async Task GenerateAsync()
    {
        try
        {
            if (_randomNumbers.Count >= 14)
            {
                _isProcessing = true;
                _uniqueSets = new();

                Random rand = new();

                while (_uniqueSets.Count < 100)
                {
                    List<int> shuffled = _randomNumbers.OrderBy(x => rand.Next()).ToList();
                    List<int> set = shuffled.Take(7).OrderBy(x => x).ToList();

                    string setKey = string.Join(",", set);

                    _uniqueSets.Add(setKey);
                }

                _isProcessing = false;

                await InvokeAsync(StateHasChanged);

            }
            else
            {
                _toastService.ShowToast($"Selected {_randomNumbers.Count} of at least 14 numbers", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ClearAsync()
    {
        _randomNumbers = new();
        _uniqueSets = new();
        await Task.CompletedTask;
    }
    private async Task ExportAsync()
    {
        try
        {
            _isProcessing = true;

            // Ensure downloads directory exists
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Format: yyyyMMddHHmmss (e.g., 20250605143000)
            DateTime now = DateTime.Now;
            string dateStr = now.ToString("yyyyMMddHHmmss");

            // File path
            string filePath = Path.Combine(downloadsPath, $"{dateStr}_RandomNumbers.csv");

            // Write CSV file
            using StreamWriter sw = new(filePath);
            {
                sw.WriteLine($"Seq,Num 1,Num 2,Num 3,Num 4,Num 5,Num 6,Num 7");

                int count = 1;

                foreach (var uniqueSet in _uniqueSets)
                {
                    sw.WriteLine($"{count++},{uniqueSet}");
                }
            }

            _toastService.ShowToast($"{Label.AppComponentRandomNumberMessage2}", Theme.Success);

            _isProcessing = false;
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
