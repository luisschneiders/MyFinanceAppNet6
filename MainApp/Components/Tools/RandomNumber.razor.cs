using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class RandomNumber : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IRandomNumberService _randomNumberService{ get; set; } = default!;

    private int _maxNumber { get; set; } = 44;
    private HashSet<int> _randomNumbers { get; set; } = new();
    private HashSet<string> _uniqueSets { get; set; } = new();
    private bool _isLoading { get; set; } = false;
    private bool _isExporting { get; set; } = false;

    public RandomNumber()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                HashSet<int> randomNumbers = await _randomNumberService.GetLocalStorageRandomNumber();

                if (randomNumbers is not null && randomNumbers.Count > 0)
                {
                    _randomNumbers = randomNumbers;
                }
            }
            catch (Exception ex)
            {
                _isLoading = false;
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }


    private async Task AddNumberAsync(int number)
    {
        if (!_randomNumbers.Add(number))
        {
            _randomNumbers.Remove(number); // Unselect it
        }

        await _randomNumberService.SetLocalStorageRandomNumber(_randomNumbers);

        await Task.CompletedTask;
    }

    private async Task GenerateAsync()
    {
        try
        {
            if (_randomNumbers.Count >= 14)
            {
                _isLoading = true;

                _uniqueSets = new();

                Random rand = new();

                while (_uniqueSets.Count < 100)
                {
                    List<int> shuffled = _randomNumbers.OrderBy(x => rand.Next()).ToList();
                    List<int> set = shuffled.Take(7).OrderBy(x => x).ToList();

                    string setKey = string.Join(",", set);

                    _uniqueSets.Add(setKey);
                }
                await Task.Delay((int)Delay.DataLoading);

                _isLoading = false;

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

        await _randomNumberService.SetLocalStorageRandomNumber(_randomNumbers);
        await Task.CompletedTask;
    }
    private async Task ExportAsync()
    {
        try
        {
            _isExporting = true;

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

            await Task.Delay((int)Delay.DataLoading);

            _isExporting = false;

            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
