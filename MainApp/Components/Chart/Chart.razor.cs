using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MainApp.Components.Chart;
using MainApp.Settings.Enum;

namespace MainApp.Components.Chart;

public partial class Chart : ComponentBase
{
    [Inject]
    private IChartService _chartService { get; set; } = default!;

    [Parameter]
	public string Id { get; set; } = string.Empty;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
	public ChartType Type { get; set; } = ChartType.Bar;

    [Parameter]
	public List<string> Data { get; set; } = default!;

    [Parameter]
    public List<string> BackgroundColor { get; set; } = default!;

    [Parameter]
    public List<string> BorderColor { get; set; } = default!;

    [Parameter]
    public List<string> Labels { get; set; } = default!;

    [Parameter]
    public EventCallback<IJSObjectReference> OnSubmitSuccess { get; set; }

    private ChartConfig _chartConfig { get; set; } = new();
    private ChartConfigDataset _chartConfigDataset { get; set; } = new();

    public Chart()
	{
	}

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _chartConfigDataset.Label = Title;
            _chartConfigDataset.Data = Data;
            _chartConfigDataset.BackgroundColor = BackgroundColor;
            _chartConfigDataset.BorderColor = BorderColor;

            _chartConfig.Type = Type.ToString().ToLower();
            _chartConfig.Data.Labels = Labels;
            _chartConfig.Data.Datasets.Add(_chartConfigDataset);

            await _chartService.InvokeChartModule();
            await _chartService.SetupChartModule(Id, _chartConfig);
        }

        await Task.CompletedTask;
    }

}
